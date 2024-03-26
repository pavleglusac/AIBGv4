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

    private int X, Z;

    private bool isDone = false;

    public RefinementTakeCommand Initialize(Player player, int takeCheap, int takeExpensive, int x, int  z)
    {
        Player = player;
        TakeCheap = takeCheap;
        TakeExpensive = takeExpensive;
        X = x;
        Z = z;

        return this;
    }

    public bool CanExecute()
    {
        House = Game.Instance.Board.FindHouse(X, Z);
        
        if (House == null) {
            Game.Instance.DisplayMessage = "Not a refinement facility!";
            return false;
        }

        if (House.IsFirstPlayers != Player.FirstPlayer)
        {
            Game.Instance.DisplayMessage = "Cannot take crystals from a refinement that is not yours";
            return false;
        }

        if (House.GetProcessedCheapCrystalCount() < TakeCheap)
        {
            Game.Instance.DisplayMessage = "You can not take more processed minerals than available in the refinement";
            return false;
        }

        if (House.GetProcessedExpensiveCrystalCount() < TakeExpensive)
        {
            Game.Instance.DisplayMessage = "You can not take more processed diamonds than available in the refinement";
            return false;
        }

        int processedCheapCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_cheap_crystal_weight"));
        int processedExpensiveCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_expensive_crystal_weight"));

        if (Player.Bag.GetRemainingCapacity() < TakeCheap * processedCheapCrystalWeight + TakeExpensive * processedExpensiveCrystalWeight)
        {
            Game.Instance.DisplayMessage = "Not enough space in your backpack for the selected crystals";
            return false;
        }

        if (!CanAct())
        {
            Game.Instance.DisplayMessage = "You must be near your refinement to take crystals from it";
            return false;
        }

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

