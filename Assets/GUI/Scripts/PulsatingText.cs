using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulsatingText : MonoBehaviour
{
    public Text text;
    readonly float pulsateDuration = 10.0f;
    private float startTime;

    void Start()
    {
        text = GetComponent<Text>();
        startTime = Time.time;
    }

    void Update()
    {
        float elapsedTime = Time.time - startTime;
        float t = Mathf.Repeat(elapsedTime, pulsateDuration) / pulsateDuration;
        float scale = 0.9f + 0.2f * Mathf.Abs(0.5f - t) * 2;
        text.transform.localScale = new Vector3(scale, scale, 1.0f);
    }
}
