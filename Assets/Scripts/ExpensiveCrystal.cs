using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpensiveCrystal : Crystal
{
    public GameObject Crystal2ParentObject { get; set; }
    public GameObject Crystal2Object { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        MaxMineHits = int.Parse(PlayerPrefs.GetString("expensive_crystal_mine_hits"));
        RemainingMineHits = MaxMineHits;
        ReplenishTurns = int.Parse(PlayerPrefs.GetString("expensive_crystal_replenish_turns"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when the player is clicked
    void OnMouseDown()
    {
        Actions.Mine(X, Z);
    }

    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }


}
