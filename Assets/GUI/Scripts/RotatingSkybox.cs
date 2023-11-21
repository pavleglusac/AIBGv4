using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSkybox : MonoBehaviour
{

    public float RotationSpeed;
    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotationSpeed);
    }
}
