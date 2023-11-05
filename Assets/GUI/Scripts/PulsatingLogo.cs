using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulsatingLogo : MonoBehaviour
{
    public Text text; // Reference to the Text component
    readonly float pulsateDuration = 4.0f; // Duration for one complete pulsation (shrink and enlarge)
    private float startTime;

    void Start()
    {
        text = GetComponent<Text>();
        startTime = Time.time;
    }

    void Update()
    {
        float elapsedTime = Time.time - startTime;
        float t = Mathf.Repeat(elapsedTime, pulsateDuration) / pulsateDuration; // Calculate a looping value within [0, 1]

        // Calculate the scaling factor for pulsation (0.9 to 1.1)
        float scale = 0.9f + 0.2f * Mathf.Abs(0.5f - t) * 2;

        // Apply the scale to the text component
        text.transform.localScale = new Vector3(scale, scale, 1.0f);
    }
}
