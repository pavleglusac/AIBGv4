using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject PlayerParentObject { get; set; }
    public GameObject PlayerObject { get; set; }
    public bool FirstPlayer { get; set; }
    public Pillar Position { get; set; }
    public int X { get; set; }
    public int Z { get; set; }

    // TODO Jovan: Added now for hud logic, change later 
    public int XP { get; set; }
    public int Coins { get; set; }
    public int Energy { get; set; }
    public string Name { get; set; }

    public void SetupPlayer(string name)
    {
        XP = 0;
        Coins = 1000;
        Energy = 100;
        Name = name;
    }


    void Start()
    {

    }


    void Update()
    {

    }


    void OnMouseDown()
    {
        // Debug.Log("Player clicked");
    }

    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }

    public void TakeEnergy(int count)
    {
        Energy -= count;
    }
}
