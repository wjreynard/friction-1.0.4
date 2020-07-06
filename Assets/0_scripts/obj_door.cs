using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_door : MonoBehaviour {

    public GameObject door;
    public float transformAmount = 6.0f;
    private bool doorIsOpening;
    private bool doorOpened;
    private Transform startPosition;

    private void Start() {
        doorIsOpening = false;
        doorOpened = false;

        // Get initial position of the door's parent
        startPosition = door.transform.parent;
        //Debug.Log(startPosition.localPosition);
    }

    void Update () {
        // If boolean is true, then translate door up for 1 second
        if (doorIsOpening) {
            door.transform.Translate(Vector3.up * Time.deltaTime * 5);
        }

        // If the y of the door is greater than 7, stop translating the door
        if (door.transform.position.y > startPosition.localPosition.y + transformAmount) {
            doorIsOpening = false;
        }
	}

    // Door opens when player enters button trigger
    void OnTriggerEnter(Collider other) {
        // "if layer == 'Player'"
        if (other.gameObject.layer == 8) {
            //Debug.Log("Door Triggered");

            // If door not opened, then play sound and open
            if (!doorOpened) {
                // Get the AudioSource of the fourth child of the PlayerAudioManager object, which happens to be sound_door
                AudioSource sound_door = GameObject.Find("PlayerAudioManager").transform.GetChild(3).GetComponent<AudioSource>();
                sound_door.Play();
                doorIsOpening = true;
            }

            // Set door to opened to stop door opening more than once
            doorOpened = true;

            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}
