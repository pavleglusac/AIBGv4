using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsPulsing : MonoBehaviour
{
    [SerializeField] private RawImage rawImage; 
    [SerializeField] private float x , y ; 
    void Start()
    {
      
    
    }

    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(x,y) * Time.deltaTime,rawImage.uvRect.size);
      
    }
}
