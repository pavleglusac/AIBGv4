using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public List<Crystal1Item> Crystal1s { get; set; } = new List<Crystal1Item>();
    public List<Crystal2Item> Crystal2s { get; set; } = new List<Crystal2Item>();
    public int Capacity { get; set; } = 15;

    public Bag()
    {
    }

    public void AddCrystal1()
    {
        Crystal1Item crystal1 = new Crystal1Item();
        if(crystal1.GetWeight() <= GetRemainingCapacity())
            Crystal1s.Add(crystal1);
    }

    public void AddCrystal2()
    {
        Crystal2Item crystal2 = new Crystal2Item();
        if(crystal2.GetWeight() <= GetRemainingCapacity())
            Crystal2s.Add(crystal2);
    }

    public void RemoveCrystal1(Crystal1Item crystal1)
    {
        Crystal1s.Remove(crystal1);
    }

    public void RemoveCrystal2(Crystal2Item crystal2)
    {
        Crystal2s.Remove(crystal2);
    }

    public bool IsFull()
    {
        return GetWeight() >= Capacity;
    }

    public bool IsEmpty()
    {
        return Crystal1s.Count == 0 && Crystal2s.Count == 0;
    } 

    public int GetWeight()
    {
        int total = 0;
        foreach (Crystal1Item crystal1 in Crystal1s)
        {
            total += crystal1.GetWeight();
        }
        foreach (Crystal2Item crystal2 in Crystal2s)
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
        Crystal1s.Clear();
        Crystal2s.Clear();
    }

    public override string ToString()
    {
        return "Bag: " + GetWeight() + "/" + Capacity + " (" + Crystal1s.Count + " Crystal1s, " + Crystal2s.Count + " Crystal2s)";
    }

}