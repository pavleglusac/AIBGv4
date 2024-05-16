using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    private readonly float x = 0.05f;
    private readonly float y = 0.1f;

    void Start()
    {
        SetHighGraphics();
        ResourceGenerator.readPrefabLevel = false;
    }


    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(x, y) * Time.deltaTime, rawImage.uvRect.size);


        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                SetHighGraphics();
                Application.targetFrameRate = 60;
            }


            if (Input.GetKeyDown(KeyCode.L))
            {
                SetLowGraphics();
                Application.targetFrameRate = 30;
            }
        }
    }

    void SetHighGraphics()
    {
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1, true);
        Application.targetFrameRate = 60;
        Debug.Log("High graphics settings applied.");
    }

    void SetLowGraphics()
    {
        QualitySettings.SetQualityLevel(0, true);
        Application.targetFrameRate = 30;
        Debug.Log("Low graphics settings applied.");
    }

}
