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
        Bag = gameObject.AddComponent<Bag>();
        Bag.Capacity = int.Parse(PlayerPrefs.GetString("backpack_default_storage_capacity"));
    }

    public string GetStats()
    {
        string stats = $@"
{Name}
Energy: {Energy}
XP: {XP}
Coins: {Coins}
Position: ({X}, {Z})
Increased backpack capacity duration: {IncreasedBackpackTurns}
Daze turns: {DazeTurns}
Frozen turns: {FrozenTurns}
Backpack capacity: {Bag.GetWeight() / Bag.Capacity}
Raw cheap crystal -> Count: {Bag.GetCountCheapRaw()}
Processed cheap crystal -> Count: {Bag.GetCountCheapProcessed()}
Raw expensive crystal -> Count: {Bag.GetCountExpensiveRaw()}
Processed expensive crystal -> Count: {Bag.GetCountExpensiveProcessed()}";
        return stats;
    }

    void Start()
    {

    }


    void Update()
    {

    }


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Actions.BuildHouse(this.X, this.Z);
        }
    }

    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }

    public void DecreaseEnergy(int amount)
    {
        Energy -= amount;
        if (Energy < 0)
            Energy = 0;
    }

    public void IncreaseEnergy(int amount)
    {
        Energy += amount;
        if (Energy > int.Parse(PlayerPrefs.GetString("max_energy")))
            Energy = int.Parse(PlayerPrefs.GetString("max_energy"));
    }

    public void InvalidMoveTakeEnergy()
    {
        Energy -= int.Parse(PlayerPrefs.GetString("invalid_turn_energy_penalty"));
    }

    public void TakeCoins(int c)
    {
        Coins -= c;
    }

    public void AddCoins(int c)
    {
        Coins += c;
    }

    public void AddXP(int xp)
    {
        XP += xp;
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
            if (!IsDazed())
            {
                HandleDaze3D(false);

            }
        }
    }

    public void AddDazeTurns()
    {
        if (!IsDazed())
        {
            HandleDaze3D(true);

        }
        DazeTurns += int.Parse(PlayerPrefs.GetString("number_of_daze_turns"));
    }

    private void HandleDaze3D(bool active)
    {

        Transform particles = PlayerObject.transform.GetChild(2);
        particles.gameObject.SetActive(active);
    }

    public void AddFrozenTurns()
    {
        if (!IsFrozen())
        {
            HandleFreeze3D(true);

        }
        FrozenTurns += int.Parse(PlayerPrefs.GetString("number_of_frozen_turns"));
    }

    private void HandleFreeze3D(bool active)
    {
        Transform firstChildTransform = PlayerObject.transform.GetChild(0);
        var icecube = firstChildTransform.GetChild(1);
        icecube.gameObject.SetActive(active);
        Transform particles = PlayerObject.transform.GetChild(1);
        particles.gameObject.SetActive(active);
    }

    public void DecreaseFrozenTurns()
    {
        if (FrozenTurns > 0)
        {
            FrozenTurns -= 1;
            if (!IsFrozen())
            {
                HandleFreeze3D(false);
            }
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
