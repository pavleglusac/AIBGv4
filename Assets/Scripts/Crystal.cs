using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal: MonoBehaviour
{
    public Pillar Position { get; set; }
    public int X { get; set; }
    public int Z { get; set; }
    public int MaxMineHits { get; set; }
    public int RemainingMineHits { get; set; }
    public int ReplenishTurns { get; set; }
    public int TurnInWhichCrystalBecameEmpty { get; set; } = -1;
    public bool IsEmpty { get; set; }
}
