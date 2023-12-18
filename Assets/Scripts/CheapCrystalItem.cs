using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapCrystalItem : MonoBehaviour
{
    public bool isProcessed { get; set; } = false;

    public int GetWeight()
    {
        return isProcessed ? 1 : 3;
    }

    public int GetValue()
    {
        return isProcessed ? 10 : 5;
    }
}