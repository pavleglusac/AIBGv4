using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class House : MonoBehaviour
{

    public GameObject HouseParentObject { get; set; }
    public GameObject HouseObject { get; set; }
    public Pillar Position { get; set; }
    public int X { get; set; }
    public int Z { get; set; }

    void Start()
    {
    }


    void Update()
    {
    }


    void OnMouseDown()
    {
        Debug.Log("Player clicked");
    }

    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }

}
