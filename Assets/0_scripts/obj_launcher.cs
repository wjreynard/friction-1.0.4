using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_launcher : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        // "if layer == 'Player'"
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<cl_player>().Launcher(transform.up);
        }
    }
}
