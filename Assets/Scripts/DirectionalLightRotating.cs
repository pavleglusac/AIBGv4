using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightRotating : MonoBehaviour
{
    public float rotationSpeed = 30f;

    // Update is called once per frame
    void Update()
    {
        // Get the current rotation
        Vector3 currentRotation = transform.eulerAngles;

        // Update only the Y-axis rotation
        currentRotation.y += rotationSpeed * Time.deltaTime;


        // Apply the new rotation
        transform.eulerAngles = currentRotation;
    }
}
