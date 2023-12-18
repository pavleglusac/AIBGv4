using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bag : MonoBehaviour
{
    public List<CheapCrystalItem> CheapCrystals { get; set; } = new List<CheapCrystalItem>();
    public List<ExpensiveCrystalItem> ExpensiveCrystals { get; set; } = new List<ExpensiveCrystalItem>();
    // private int Capacity  = int.Parse(PlayerPrefs.GetString("backpack_default_storage_capacity"));
    private int Capacity;

    // public Bag()
    // {
    //     Capacity = 10;
    // }

    private void Start()
    {
        Capacity = int.Parse(PlayerPrefs.GetString("backpack_default_storage_capacity"));
    }

    public void IncreaseBagCapacity()
    {
        Capacity += int.Parse(PlayerPrefs.GetString("increase_in_backpack_storage_capacity"));
    }

    public void DecreaseBagCapacity()
    {
        Capacity -= int.Parse(PlayerPrefs.GetString("decrease_in_backpack_storage_capacity"));
    }

    public void AddCheapCrystal()
    {
        Debug.Log(Capacity);
        CheapCrystalItem crystal1 = new CheapCrystalItem();
        if(crystal1.GetWeight() <= GetRemainingCapacity())
            CheapCrystals.Add(crystal1);
    }

    public void AddExpensiveCrystal()
    {
        ExpensiveCrystalItem crystal2 = new ExpensiveCrystalItem();
        if(crystal2.GetWeight() <= GetRemainingCapacity())
            ExpensiveCrystals.Add(crystal2);
    }

    public void RemoveCheapCrystal(CheapCrystalItem crystal1)
    {
        CheapCrystals.Remove(crystal1);
    }

    public void RemoveExpensiveCrystal(ExpensiveCrystalItem crystal2)
    {
        ExpensiveCrystals.Remove(crystal2);
    }

    public bool IsFull()
    {
        return GetWeight() >= Capacity;
    }

    public bool IsEmpty()
    {
        return CheapCrystals.Count == 0 && ExpensiveCrystals.Count == 0;
    } 

    public int GetWeight()
    {
        int total = 0;
        foreach (CheapCrystalItem crystal1 in CheapCrystals)
        {
            total += crystal1.GetWeight();
        }
        foreach (ExpensiveCrystalItem crystal2 in ExpensiveCrystals)
        {
            total += crystal2.GetWeight();
        }
        return total;
    }

    public int GetRemainingCapacity()
    {
        return Capacity - GetWeight();
    }

    public void Empty()
    {
        CheapCrystals.Clear();
        ExpensiveCrystals.Clear();
    }

    public override string ToString()
    {
        return "Bag: " + GetWeight() + "/" + Capacity + " (" + CheapCrystals.Count + " CheapCrystals, " + ExpensiveCrystals.Count + " ExpensiveCrystals)";
    }

}