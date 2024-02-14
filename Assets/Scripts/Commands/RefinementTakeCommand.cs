using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RefinementTakeCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }
    public int TakeCheap { get; set; }
    public int TakeExpensive { get; set; }
    public House House { get; set; }

    private bool isDone = false;

    public RefinementTakeCommand Initialize(Player player, int takeCheap, int takeExpensive, House house)
    {
        Player = player;
        TakeCheap = takeCheap;
        TakeExpensive = takeExpensive;
        House = house;
        return this;
    }

    public bool CanExecute()
    {
        if (House.IsFirstPlayers != Player.FirstPlayer) return false;
        if (House.GetProcessedCheapCrystalCount() < TakeCheap) return false;
        if (House.GetProcessedExpensiveCrystalCount() < TakeExpensive) return false;
        int processedCheapCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_cheap_crystal_weight"));
        int processedExpensiveCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_expensive_crystal_weight"));
        if (Player.Bag.GetRemainingCapacity() < TakeCheap * processedCheapCrystalWeight + TakeExpensive * processedExpensiveCrystalWeight) return false;
        if (!CanAct()) return false;
        return true;
    }

    public bool CanAct()
    {
        List<Pillar> neighbours = Game.Instance.Board.getNeighbours(House.Position);
        return neighbours.Contains(Player.Position);
    }

    public void Execute()
    {
        for (int i = 0; i < TakeCheap; i++)
        {
            Player.Bag.AddCheapCrystal(House.PopProcessedCheapCrystal());
        }

        for (int i = 0; i < TakeExpensive; i++)
        {
            Player.Bag.AddExpensiveCrystal(House.PopProcessedExpensiveCrystal());
        }
        Game.Instance.DisplayMessage = "Refinement take successful!";
        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }
}

