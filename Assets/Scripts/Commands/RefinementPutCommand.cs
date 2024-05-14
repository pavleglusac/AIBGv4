﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RefinementPutCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }
    public int PutCheap { get; set; }
    public int PutExpensive { get; set; }
    public House House { get; set; }
    public int X, Z;
    public bool isDone { get; set; } = false;


    public RefinementPutCommand Initialize(Player player, int putCheap, int putExpensive, int x, int z)
    {
        Player = player;
        PutCheap = putCheap;
        PutExpensive = putExpensive;
        X = x;
        Z = z;
        return this;
    }


    public bool CanExecute()
    {

        if (X < 0 || X >= Game.Instance.Board.Width || Z < 0 || Z >= Game.Instance.Board.Height)
        {
            Game.Instance.DisplayMessage = "Invalid coordinates for attack!";
            return false;
        }

        if (PutCheap == 0 && PutExpensive == 0)
        {
            Game.Instance.DisplayMessage = "You must put in at least one item!";
            return false;
        }

        House = Game.Instance.Board.FindHouse(X, Z);

        if (House == null)
        {
            Game.Instance.DisplayMessage = "Not a refinement facility!";
            return false;
        }

        if (House.IsFirstPlayers != Player.FirstPlayer)
        {
            Game.Instance.DisplayMessage = "Can not put in refinement that is not yours";
            return false;
        }
        if (Player.Bag.GetCheapCrystalCount() < PutCheap)
        {
            Game.Instance.DisplayMessage = "You can't put more minerals than you have in your backpack";

            return false;
        }
        if (Player.Bag.GetExpensiveCrystalCount() < PutExpensive)
        {
            Game.Instance.DisplayMessage = "You can't put more diamonds than you have in your backpack";
            return false;
        }
        if (!CanAct())
        {
            Game.Instance.DisplayMessage = "You must be near your refinery to put the crystals inside";
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
        for (int i = 0; i < PutCheap; i++)
        {
            House.CheapCrystals.Add(new Tuple<CheapCrystalItem, int>(Player.Bag.PopCheapCrystal(), Game.Instance.TurnCount));
        }

        for (int i = 0; i < PutExpensive; i++)
        {
            House.ExpensiveCrystals.Add(new Tuple<ExpensiveCrystalItem, int>(Player.Bag.PopExpensiveCrystal(), Game.Instance.TurnCount));
        }
        Game.Instance.DisplayMessage = "Crystals have successfully been put into the refinery";

        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }
}

