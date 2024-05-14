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
        Application.targetFrameRate = 60;
    }


    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(x, y) * Time.deltaTime, rawImage.uvRect.size);

        if (Input.GetKeyDown(KeyCode.H))
        {
            SetHighGraphics();
            Application.targetFrameRate = 60;
        }

        // Check for low graphics setting key press (L)
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetLowGraphics();
            Application.targetFrameRate = 30;
        }
    }

    void SetHighGraphics()
    {
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1, true);
        Debug.Log("High graphics settings applied.");
    }

    void SetLowGraphics()
    {
        QualitySettings.SetQualityLevel(0, true);
        Debug.Log("Low graphics settings applied.");
    }

}
