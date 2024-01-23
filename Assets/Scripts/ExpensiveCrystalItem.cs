using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExpensiveCrystalItem : MonoBehaviour
{
    public bool isProcessed { get; set; } = false;
    public DateTime TimeAdded { get; set; } = DateTime.Now;
    public int GetWeight()
    {
        return isProcessed ? 1 : 3;
    }

    public int GetValue()
    {
        return isProcessed ? 10 : 5;
    }
}