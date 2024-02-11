using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class House : MonoBehaviour
{


    public GameObject HouseParentObject { get; set; }
    public GameObject HouseObject { get; set; }
    public Pillar Position { get; set; }
    public bool IsFirstPlayers { get; set; }
    public int X { get; set; }
    public int Z { get; set; }
    public List<Tuple<CheapCrystalItem, int>> CheapCrystals { get; set; } = new List<Tuple<CheapCrystalItem, int>>();
    public List<Tuple<ExpensiveCrystalItem, int>> ExpensiveCrystals { get; set; } = new List<Tuple<ExpensiveCrystalItem, int>>();

    void Start()
    {
    }


    void Update()
    {
    }


    void OnMouseDown()
    {
        if (Game.Instance.GetCurrentPlayer().FirstPlayer != this.IsFirstPlayers) return;
        Game.Instance.selectedHouse = this;
        RefinementMenuNavigation.Instance.OpenRefinementMenu();

    }

    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }

    public int GetProcessedCheapCrystalCount()
    {
        int cheapCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("cheap_crystal_processing_turns"));
        return CheapCrystals.Count(crystal => Game.Instance.TurnCount - crystal.Item2 >= cheapCrystalProcessingTurns);
    }

    public int GetUnprocessedCheapCrystalCount()
    {
        int cheapCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("cheap_crystal_processing_turns"));
        return CheapCrystals.Count(crystal => Game.Instance.TurnCount - crystal.Item2 < cheapCrystalProcessingTurns);
    }

    public int GetProcessedExpensiveCrystalCount()
    {
        int expensiveCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("expensive_crystal_processing_turns"));
        return ExpensiveCrystals.Count(crystal => Game.Instance.TurnCount - crystal.Item2 >= expensiveCrystalProcessingTurns);
    }

    public int GetUnprocessedExpensiveCrystalCount()
    {
        int expensiveCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("expensive_crystal_processing_turns"));
        return ExpensiveCrystals.Count(crystal => Game.Instance.TurnCount - crystal.Item2 < expensiveCrystalProcessingTurns);
    }






}
