using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Bag : MonoBehaviour
{
    public List<CheapCrystalItem> CheapCrystals { get; set; } = new List<CheapCrystalItem>();
    public List<ExpensiveCrystalItem> ExpensiveCrystals { get; set; } = new List<ExpensiveCrystalItem>();
    // private int Capacity  = int.Parse(PlayerPrefs.GetString("backpack_default_storage_capacity"));
    public int Capacity { get; set; }

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

    public void ResetBagCapacity()
    {
        Capacity = int.Parse(PlayerPrefs.GetString("backpack_default_storage_capacity"));
    }

    public void AddCheapCrystal()
    {
        CheapCrystalItem crystal1 = new CheapCrystalItem();
        if (crystal1.GetWeight() <= GetRemainingCapacity())
            CheapCrystals.Add(crystal1);
    }

    public void AddCheapCrystal(CheapCrystalItem crystal1)
    {
        if (crystal1.GetWeight() <= GetRemainingCapacity())
            CheapCrystals.Add(crystal1);
    }


    public void AddExpensiveCrystal()
    {
        ExpensiveCrystalItem crystal2 = new ExpensiveCrystalItem();
        if (crystal2.GetWeight() <= GetRemainingCapacity())
            ExpensiveCrystals.Add(crystal2);
    }

    public void AddExpensiveCrystal(ExpensiveCrystalItem crystal2)
    {
        if (crystal2.GetWeight() <= GetRemainingCapacity())
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
    public void RemoveLastInsertedCrystal()
    {
        if (ExpensiveCrystals.Count > 0 || CheapCrystals.Count > 0)
        {
            // Check which list has the latest added crystal
            bool expensiveLast = ExpensiveCrystals.Count > 0 &&
                                 (CheapCrystals.Count == 0 ||
                                  ExpensiveCrystals[ExpensiveCrystals.Count - 1].TimeAdded > CheapCrystals[CheapCrystals.Count - 1].TimeAdded);

            if (expensiveLast)
            {
                ExpensiveCrystals.RemoveAt(ExpensiveCrystals.Count - 1);
            }
            else
            {
                CheapCrystals.RemoveAt(CheapCrystals.Count - 1);
            }

        }

    }

    public int GetCheapCrystalCount()
    {
        return CheapCrystals.Count;
    }

    public int GetExpensiveCrystalCount()
    {
        return ExpensiveCrystals.Count;
    }

    public CheapCrystalItem PopCheapCrystal()
    {
        CheapCrystalItem crystal = CheapCrystals[0];
        CheapCrystals.RemoveAt(0);
        return crystal;
    }

    public ExpensiveCrystalItem PopExpensiveCrystal()
    {
        ExpensiveCrystalItem crystal = ExpensiveCrystals[0];
        ExpensiveCrystals.RemoveAt(0);
        return crystal;
    }

        public float GetTotalWeightExpensiveProcessed()
    {
        return ExpensiveCrystals.Where(crystal => crystal.isProcessed).Sum(crystal => crystal.GetWeight());
    }

    public float GetTotalWeightExpensiveRaw()
    {
        return ExpensiveCrystals.Where(crystal => !crystal.isProcessed).Sum(crystal => crystal.GetWeight());
    }

    public float GetTotalWeightCheapProcessed()
    {
        return CheapCrystals.Where(crystal => crystal.isProcessed).Sum(crystal => crystal.GetWeight());
    }

    public float GetTotalWeightCheapRaw()
    {
        return CheapCrystals.Where(crystal => !crystal.isProcessed).Sum(crystal => crystal.GetWeight());
    }

    public int GetCountExpensiveProcessed()
    {
        return ExpensiveCrystals.Count(crystal => crystal.isProcessed);
    }

    public int GetCountExpensiveRaw()
    {
        return ExpensiveCrystals.Count(crystal => !crystal.isProcessed);
    }

    public int GetCountExpensive()
    {
        return ExpensiveCrystals.Count;
    }

    public int GetCountCheap()
    {
        return CheapCrystals.Count;
    }

    public int GetCountCheapProcessed()
    {
        return CheapCrystals.Count(crystal => crystal.isProcessed);
    }

    public int GetCountCheapRaw()
    {
        return CheapCrystals.Count(crystal => !crystal.isProcessed);
    }

}