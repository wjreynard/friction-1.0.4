using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cl_pitch : MonoBehaviour {
	private Transform tform;
	private float y;
	private int invert = -1;
	public float sensi;
	public Text yoursensi;
	void Start() {
		tform = GetComponent<Transform>();
		y = 0f;
		invert = invert == 0 ? -1 : 1; // make sure it's not 0;
		//if (PlayerPrefs.HasKey("invert")) invert = PlayerPrefs.GetInt("invert");
		//if (PlayerPrefs.HasKey("sensi")) sensi = PlayerPrefs.GetFloat("sensiY");
	}

	void FixedUpdate() {
			sensi += Input.GetAxis("sensiChange");
	}
	void Update() {
		yoursensi.text = sensi.ToString("f3");
		if (Input.GetButtonDown("Invert")) invert *= -1;

		

		y += invert * sensi * Input.GetAxisRaw("Mouse Y");
		y = Mathf.Clamp(y, -89.9f, 89.9f);
		tform.localEulerAngles = new Vector3(y, 0, 0);
	}
}
