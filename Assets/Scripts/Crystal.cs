using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Crystal : MonoBehaviour
{
    public Pillar Position { get; set; }
    public int X { get; set; }
    public int Z { get; set; }
    public int MaxMineHits { get; set; }
    public int RemainingMineHits { get; set; }
    public int ReplenishTurns { get; set; }
    public int TurnInWhichCrystalBecameEmpty { get; set; } = -1;
    public bool IsEmpty { get; set; }

    void OnMouseOver()
    {
        // Added so that players can not click 3D objects trough UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Actions.BuildHouse(this.X, this.Z);
        }
    }

    public int CalculateReplenishTurn()
    {
        if (IsEmpty)
        {
            return ReplenishTurns - (Game.Instance.TurnCount - TurnInWhichCrystalBecameEmpty);
        }
        else
        {
            return 0;
        }
    }
}
