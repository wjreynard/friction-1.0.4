using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_collectible_boid : MonoBehaviour {
    
    private float startTime;

    void Start()
    {
        startTime = Time.time;

        // Destroy boid after lifetime
        Destroy(gameObject, 1);
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * 0.1f;

        // Shrink
        if (transform.localScale.x > 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, distCovered);
        }
    }
}