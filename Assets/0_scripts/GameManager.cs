//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        if (index == 1)
        {
            audioManager.FadeInSound("Music");
        }
        else if (index == 0 || index == 5)
        {
            PlayerPrefs.SetFloat("Collectibles", 0.1f);
            PlayerPrefs.SetFloat("playerSens", 1.0f);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
