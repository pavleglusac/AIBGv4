using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.ShaderKeywordFilter;

public class ExpensiveCrystalItem : MonoBehaviour
{
    public bool isProcessed { get; set; } = false;
    public DateTime TimeAdded { get; set; } = DateTime.Now;
    public int GetWeight()
    {
        int processedExpensiveCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_expensive_crystal_weight"));
        int rawExpensiveCrystalWeight = int.Parse(PlayerPrefs.GetString("raw_expensive_crystal_weight"));
        return isProcessed ? processedExpensiveCrystalWeight : rawExpensiveCrystalWeight;
    }

}