using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    public float yThreshold = -5.3f; // Y-position threshold for destroying the asteroid

    void Update()
    {
        // Check if the asteroid has fallen below the specified threshold
        if (transform.position.y <= yThreshold)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }
}
