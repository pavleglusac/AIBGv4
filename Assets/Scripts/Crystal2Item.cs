using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal2Item : MonoBehaviour
{
    public bool isProcessed { get; set; } = false;

    public int GetWeight()
    {
        return isProcessed ? 1 : 3;
    }
}