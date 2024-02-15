using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapCrystal : Crystal
{
    public GameObject Crystal1ParentObject { get; set; }
    public GameObject Crystal1Object { get; set; }


    void Start()
    {
        MaxMineHits = int.Parse(PlayerPrefs.GetString("cheap_crystal_mine_hits"));
        RemainingMineHits = MaxMineHits;
        ReplenishTurns = int.Parse(PlayerPrefs.GetString("cheap_crystal_replenish_turns"));
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
