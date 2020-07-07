//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
	void Start () {
        // On game start, initialise...
        PlayerPrefs.SetFloat("Collectibles", 0.1f);
        PlayerPrefs.SetFloat("playerSens", 1.0f);

        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 5 || index == 0)
        {
            Debug.Log("main menu");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
