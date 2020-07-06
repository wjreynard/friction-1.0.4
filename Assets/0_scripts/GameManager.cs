using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
	void Start () {
        // On game start, initialise...
        PlayerPrefs.SetFloat("Collectibles", 0.1f);
        PlayerPrefs.SetFloat("playerSens", 1.0f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
