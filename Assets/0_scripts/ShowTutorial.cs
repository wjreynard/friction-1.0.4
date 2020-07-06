using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTutorial : MonoBehaviour {

    public Image tutImg;

    private void OnTriggerEnter(Collider other)
    {
        // "if layer == 'Player'"
        if (other.gameObject.layer == 8)
        {
            tutImg.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // "if layer == 'Player'"
        if (other.gameObject.layer == 8)
        {
            tutImg.enabled = false;
        }
    }
}
