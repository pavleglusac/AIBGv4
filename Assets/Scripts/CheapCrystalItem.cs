using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapCrystalItem : MonoBehaviour
{
    public bool isProcessed { get; set; } = false;
    public DateTime TimeAdded { get; set; } = DateTime.Now;

    public int GetWeight()
    {
        int processedCheapCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_cheap_crystal_weight"));
        int rawCheapCrystalWeight = int.Parse(PlayerPrefs.GetString("raw_cheap_crystal_weight"));
        return isProcessed ? processedCheapCrystalWeight : rawCheapCrystalWeight;
    }

}