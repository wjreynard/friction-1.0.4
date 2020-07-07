using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class cl_player : MonoBehaviour {
    [Header("Player")]
    public float PhysGravity = 9.807f;

    // Player View
    public float playerViewYOffset;
    private Transform playerView;
    private Transform tform;
    public float sensitivity;
    public float gravity = 16f;
    public float friction = 3.0f;
    private float frictionMulti;
    
    [Header("Movement")]
    public float moveSpeed = 3.2f;
    public float runAcceleration = 20.0f; // Originally 20.0f;
    public float runDeacceleration = 5.0f; // Originally 5.0f;
    //public float airAcceleration		= 2.0f;
    //public float airDeacceleration	= 2.0f;
    //public float airControl			= 0.3f;
    public float sideStrafeAcceleration = 1000.0f;
    public float sideStrafeSpeed = 0.2f;
    public float jumpSpeed = 6.0f;
    public float moveScale = 1.0f;
    
    [Header("Collectibles")]
    public float collectibleCount; // Integer to store the number of pickups collected so far.
    public ParticleSystem collectibleTrail;
    public GameObject obj_collectible_boid;
    
    [Header("Sound")]
    private AudioSource sound_launcher;
    private AudioSource sound_jump;
    private RandomSound sound_collectibles;
    private bool alreadyLanded = false;
    
    [Header("Ramps")]
    public float downLandBoost = 0.5f;
    public float downJumpBoost = 0.4f;
    public float upJumpBoost = 0.15f;
    public float upSlow = 0.3f;
    
    // Camera rotationals
    private float rotX;
    private float rotY;

    private Vector3 moveDirection;
    private Vector3 playerVelocity;
    private float playerTopVelocity;

    // If true then the player is fully on the ground
    private bool grounded = false;

    // Players can queue the next jump just before she hits the ground
    private bool wishJump = false;
    RaycastHit hit;

    private float playerFriction;
    private CharacterController controller;

    [Header("UI")]
    public Text speed;
    public Text sensi;
    public Image speedometer;

    private float forwardmove;
    private float rightmove;
    private float upmove;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Physics.gravity = new Vector3(0, -PhysGravity, 0);

        tform = GetComponent<Transform>();
        playerView = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        
        rotY = 0.0f;

        // Initialise player's collectible trail
        collectibleCount = PlayerPrefs.GetFloat("Collectibles");
        ParticleSystem.MainModule trails = collectibleTrail.main;
        trails.startSizeMultiplier = collectibleCount * 0.002f;

        // Set sensitivity
        sensitivity = PlayerPrefs.GetFloat("playerSens");
    }

    void Update() {
        // Checking if just landed, so sound_jump does not loop
        if (controller.isGrounded && alreadyLanded) {

            // Get the AudioSource of the second child of the PlayerAudioManager object, which happens to be sound_jump
            AudioSource sound_jump = GameObject.Find("PlayerAudioManager").transform.GetChild(1).GetComponent<AudioSource>();

            // Set sound_jump volume based on player velocity
            Vector3 ups;
            ups = controller.velocity;
            float upsMag = Mathf.Floor(ups.magnitude);
            sound_jump.volume = 0.01f + upsMag * 0.015f;

            sound_jump.Play();

            alreadyLanded = false;
        }
        
        // Returns player to menu
        if (Input.GetButtonDown("Cancel")) {
            SceneManager.LoadScene(0);
        }
        
        // Check ground below
        Ray ray = new Ray(tform.position, -tform.up);
        Physics.Raycast(ray, out hit, 1f);
        Debug.DrawLine(hit.point, hit.normal.normalized);
        forwardmove = Input.GetAxisRaw("Vertical");
        rightmove = Input.GetAxisRaw("Horizontal");
        
        // Camera Rotation
        rotX += Input.GetAxisRaw("Mouse Y") * sensitivity * -1;
        rotY += Input.GetAxisRaw("Mouse X") * sensitivity;

        // Clamp the X rotation
        if (rotX < -90)
            rotX = -90;
        else if (rotX > 90)
            rotX = 90;

        tform.rotation = Quaternion.Euler(0, rotY, 0); // Rotates the collider
        playerView.rotation = Quaternion.Euler(rotX, rotY, 0);
        
        // Movement
        QueueJump();
        if (controller.isGrounded) GroundMove();
        else AirMove();
        controller.Move(playerVelocity * Time.deltaTime);
        
        // Uncomment the below line for jumping in the air.
        //if (Input.GetButtonDown("Jump")) playerVelocity.y = jumpSpeed;
    }

    // Acceleration function that is called by AirMove(), GroundMove() and OnTriggerEnter().
    public void Accelerate(Vector3 wishdir, float wishspeed, float accel) {
		    float addspeed;
		    float accelspeed;
		    float currentspeed;

		    currentspeed = Vector3.Dot(playerVelocity, wishdir);
		    addspeed = wishspeed - currentspeed;
		    if (addspeed <= 0) return;
		    accelspeed = accel * wishspeed * Time.deltaTime;
		    if (accelspeed > addspeed) accelspeed = addspeed;

		    playerVelocity.x += accelspeed * wishdir.x;
		    playerVelocity.z += accelspeed * wishdir.z;
		    //playerVelocity.y += accelspeed * wishdir.y;
	    }

    // Happens when the player is in the air.
    void AirMove() {
		Vector3 wishdir;
		float accel;

		float scale = CmdScale();

		wishdir = new Vector3(rightmove, 0, forwardmove);
		wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();

        float wishspeed = wishdir.magnitude * moveSpeed * scale;

		if (wishspeed > sideStrafeSpeed) wishspeed = sideStrafeSpeed;
		
		float turnAngle = Vector3.Dot(wishdir, new Vector3(playerVelocity.x, 0, playerVelocity.z).normalized * 1.3f);
		accel = sideStrafeAcceleration * (1f - Mathf.Clamp(Mathf.Abs(turnAngle), 0, 0.9f));
		//drag.text = (turnAngle * turnAngle).ToString();
		Accelerate(wishdir, wishspeed, accel);

		//playerVelocity += gravity * Vector3.down * Time.deltaTime;
		playerVelocity.y -= gravity * Time.deltaTime;

		if (controller.collisionFlags == CollisionFlags.Above && playerVelocity.y > 0)
			playerVelocity.y -= 100f * Time.deltaTime ;
	}

    // Happens when the player is on the ground.
    void GroundMove() {
		Vector3 wishdir;

		// Do not apply friction if the player is queuing up the next jump
		if (!wishJump) {
			frictionMulti = 1.0f;
			Invoke("ApplyFriction", 0.2f);
		} else {
			frictionMulti = 0.0f;
			ApplyFriction();
		}

		wishdir = new Vector3(rightmove, 0, forwardmove);
		wishdir = transform.TransformDirection(wishdir);
		wishdir.Normalize();

		float wishspeed = wishdir.magnitude * moveSpeed;

		Accelerate(wishdir, wishspeed, runAcceleration);

        // Jumping
        if (wishJump) {
            alreadyLanded = true;
            
            wishJump = false;

            float oldYvel = playerVelocity.y;
            Vector2 inVelHor = new Vector2(playerVelocity.x, playerVelocity.z).normalized;
            Vector2 hitNormalHor = new Vector2(hit.normal.x, hit.normal.z);
            float rampDir = Vector3.Dot(inVelHor, hitNormalHor);

            if (playerVelocity.y < 0) playerVelocity.y = 0;

            Vector3 horVel = new Vector3(playerVelocity.x, 0, playerVelocity.z);

            if (rampDir > 0)
            {
                playerVelocity += horVel.normalized * Mathf.Abs(oldYvel) * rampDir * downLandBoost;
            }
            else if (rampDir < 0)
                playerVelocity -= horVel.normalized * Mathf.Abs(oldYvel) * Mathf.Abs(rampDir) * upSlow;

            playerVelocity.y = jumpSpeed * (1 - rampDir * playerVelocity.magnitude * upJumpBoost)
                + rampDir * Mathf.Abs(oldYvel) * downJumpBoost;
        }
        else {
            playerVelocity.y = 0f;
        }
	}

    public void Launcher(Vector3 launchDirection) {
        Debug.Log("Player Triggered Launcher");

        // Get the AudioSource of the third child of the PlayerAudioManager object, which happens to be sound_launcher
        AudioSource sound_launcher = GameObject.Find("PlayerAudioManager").transform.GetChild(2).GetComponent<AudioSource>();
        sound_launcher.Play();

        //playerVelocity.y += 800 * Time.deltaTime;
        playerVelocity += launchDirection * 800 * Time.deltaTime;
    }

    public void BoosterZ(Vector3 boostDirection) {
        boostDirection = transform.TransformDirection(boostDirection);
        boostDirection.Normalize();
        
        playerVelocity.z += 400 * Time.deltaTime * boostDirection.z;
    }

    //----------------------------------------------------------------------------------------------------//
    void OnTriggerEnter(Collider other)
    {
        // "if layer == 'Collectible'"
        if (other.gameObject.layer == 13) {
            // Add to collectible count
            collectibleCount++;

            // Play sound from sound_collectibles array
            RandomSound sound_collectibles = GameObject.Find("PlayerAudioManager").transform.GetChild(0).GetComponent<RandomSound>();
            sound_collectibles.RandomPlay();
            
            // Instanstiate smaller, shrinking collectible for visual effect
            Instantiate(obj_collectible_boid, other.transform.position, other.transform.rotation);

            // Add to collectible trail size
            ParticleSystem.MainModule trails = collectibleTrail.main;
            trails.startSizeMultiplier = collectibleCount * 0.005f;

            // Destroy collectible
            Destroy(other.gameObject);
        }

        // "if layer == 'End Pillar'"
        if (other.gameObject.layer == 14) {
            // When player reaches end of the level, update they're collectible count in PlayerPrefs to be accessed on next level initialisation
            float collectibles = PlayerPrefs.GetFloat("Collectibles");
            collectibles += collectibleCount;
            PlayerPrefs.SetFloat("Collectibles", collectibles);
        }
    }

    // Applies friction to the player, called in both the air and on the ground
    void ApplyFriction() {
		Vector3 vec = playerVelocity;
		float speed;
		float newspeed;
		float control;
		float drop;

		vec.y = 0.0f;
		speed = vec.magnitude;
		drop = 0.0f;

		// Apply friction only if the player is on the ground
		if (controller.isGrounded) {
			control = speed < runDeacceleration ? runDeacceleration : speed;
			drop = control * friction * frictionMulti * Time.deltaTime;
		}
		newspeed = speed - drop;
		playerFriction = newspeed;
		if (newspeed < 0) newspeed = 0;
		if (speed != 0) 
			newspeed /= speed;

		playerVelocity.x *= newspeed;
		playerVelocity.z *= newspeed;
		//playerVelocity.y *= newspeed;
	}

    // Allows the player to hold the "Jump" key to jump indefinitely
    void QueueJump() {
		if (Input.GetButton("Jump") && !wishJump) wishJump = true;
		if (Input.GetButtonUp("Jump")) wishJump = false;
    }

     private float CmdScale() {
		int max;
		float total;
		float scale;

		max = (int)Mathf.Abs(forwardmove);
		if ((int)Mathf.Abs(rightmove) > max) {
			max = (int)Mathf.Abs(rightmove);
		}
		if ((int)Mathf.Abs(upmove) > max) {
			max = (int)Mathf.Abs(upmove);
		}
		if (max <= 0) {
			return 0;
		}

		total = Mathf.Sqrt(forwardmove * forwardmove + rightmove * rightmove);
		scale = moveSpeed * max / (moveScale * total);

		return scale;
	}

    // Displays the player's speed on the UI
    void OnGUI() {
		Vector3 ups;
		ups = controller.velocity;
        //ups.y = 0;
        float upsMag = Mathf.Floor(ups.magnitude * 100);

        //speed.text = (ups * 100).magnitude.ToString();
        speed.text = upsMag.ToString() + " u/s";

        float fill = ExtensionMethods.LinearRemap(ups.magnitude, 0, 25, 0, 1);
        speedometer.fillAmount = fill;
	}
}

