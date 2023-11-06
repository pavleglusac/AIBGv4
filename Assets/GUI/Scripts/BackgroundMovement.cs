using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    private readonly float x = 0.1f;
    private readonly float y = 0.2f;

    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(x, y) * Time.deltaTime, rawImage.uvRect.size);
    }
}
