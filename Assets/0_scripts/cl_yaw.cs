using UnityEngine;
using System.Collections;

public class cl_yaw : MonoBehaviour {
	private Transform tform, player;
	private float h;
	public float sensi, height, range;
	
	void Start() {
		tform = GetComponent<Transform>();
		player = GameObject.Find("Player").GetComponent<Transform>();
		h = 0f;
		//sensi = PlayerPrefs.GetFloat("sensi");
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update() {
		tform.position = new Vector3(player.transform.position.x, 
			player.transform.position.y, player.transform.position.z);
		h = Input.GetAxisRaw("Mouse X");
		tform.Rotate(new Vector3(0, h * sensi, 0));
	}
}
