using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
	void Start () {
        // On game start, initialise...
        //PlayerPrefs.GetFloat("playerSens", 1); // Set to default of one
        PlayerPrefs.SetFloat("Collectibles", 0.1f);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
