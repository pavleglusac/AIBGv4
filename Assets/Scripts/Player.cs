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
    public Bag Bag { get; set; }

    // TODO Jovan: Added now for hud logic, change later 
    public int XP { get; set; }
    public int Coins { get; set; }
    public int Energy { get; set; }
    public string Name { get; set; }

    public int DazeTurns { get; set; } = 0;
    public int FrozenTurns { get; set; } = 0;
    public int IncreasedBackpackTurns { get; set; } = 0;

    public void SetupPlayer(string name)
    {
        XP = int.Parse(PlayerPrefs.GetString("start_xp"));
        Coins = int.Parse(PlayerPrefs.GetString("start_coins"));
        Energy = int.Parse(PlayerPrefs.GetString("start_energy"));
        Name = name;
    }

    void Start()
    {
        Bag = gameObject.AddComponent<Bag>();
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

    public void InvalidMoveTakeEnergy()
    {
        Energy -= int.Parse(PlayerPrefs.GetString("invalid_turn_energy_penalty"));
    }

    public void TakeCoins(int count)
    {
        Coins -= count;
    }

    public bool IsFrozen()
    {
        return FrozenTurns > 0;
    }
    public bool IsDazed()
    {
        return DazeTurns > 0;
    }

    public void DecreaseDazeTurns()
    {
        if (DazeTurns > 0)
        {
            DazeTurns -= 1;
        }
    }

    public void AddDazeTurns()
    {
        DazeTurns += int.Parse(PlayerPrefs.GetString("number_of_daze_turns"));
    }

    public void AddFrozenTurns()
    {
        FrozenTurns += int.Parse(PlayerPrefs.GetString("number_of_frozen_turns"));
    }

    public void DecreaseFrozenTurns()
    {
        if (FrozenTurns > 0)
        {
            FrozenTurns -= 1;
        }
    }
    public void AddIncreasedBackpackStorageTurns()
    {
        IncreasedBackpackTurns += int.Parse(PlayerPrefs.GetString("number_of_bigger_backpack_turns"));
        Bag.IncreaseBagCapacity();
    }
    public void DecreaseIncreasedBackpackStorageTurns()
    {
        if (IncreasedBackpackTurns > 0)
        {
            IncreasedBackpackTurns -= 1;
        }
        else
        {
            Bag.ResetBagCapacity();
            while (Bag.GetWeight() > int.Parse(PlayerPrefs.GetString("backpack_default_storage_capacity")))
            {
                Bag.RemoveLastInsertedCrystal();
            }
        }
    }


}
