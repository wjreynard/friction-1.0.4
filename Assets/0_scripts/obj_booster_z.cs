using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_booster_z : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        // "if layer == 'Player'"
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<cl_player>().BoosterZ(transform.forward);
        }
    }
}
