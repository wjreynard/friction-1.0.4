using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class cl_misc : MonoBehaviour {
	private string scene;
	void Update () {
		scene = SceneManager.GetActiveScene().name;
		if (Input.GetButtonDown("Restart")) SceneManager.LoadScene(scene);

		if (Input.GetKeyDown("z")) Application.targetFrameRate = 30;
		if (Input.GetKeyDown("x")) Application.targetFrameRate = 240;
	}
}
