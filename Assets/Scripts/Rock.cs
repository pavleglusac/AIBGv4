using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject RockParentObject {get; set;}
    public GameObject RockObject {get; set;}
    public Pillar Position { get; set; }
    public int X {get; set;}
    public int Z {get; set;}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when the player is clicked
    void OnMouseDown()
    {
        Debug.Log(X + Z);
        Position.Move();
    }


    private void OnMouseEnter()
    {
        Debug.Log("rock enter");
    }

    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }
}
