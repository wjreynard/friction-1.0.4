using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple script that lerps the size of the collectible
public class obj_collectible : MonoBehaviour {

    private float time;
    private float speed;

    private void Start() {
        time = 0;
        speed = 1.5f;
    }

    private void Update() {
        time += speed * Time.deltaTime;

        Vector3 originalSize = new Vector3(0.25f, 0.25f, 0.25f);
        Vector3 newSize = new Vector3(0.35f, 0.35f, 0.35f);

        transform.localScale = Vector3.Lerp(originalSize, newSize, Mathf.PingPong(time, 1));
    }
}
