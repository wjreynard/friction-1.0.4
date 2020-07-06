using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_base : MonoBehaviour {

    public Transform spawnTransform;
    private Vector3 spawnPoint;

    private void Start()
    {
        spawnPoint.x = spawnTransform.position.x;
        spawnPoint.y = spawnTransform.position.y;
        spawnPoint.z = spawnTransform.position.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("Player triggered base");
            other.gameObject.transform.position = spawnPoint;
        }
    }
}
