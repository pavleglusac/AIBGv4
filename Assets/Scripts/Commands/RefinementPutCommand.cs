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

    private bool isDone = false;

    public RefinementPutCommand Initialize(Player player, int putCheap, int putExpensive, House house)
    {
        Player = player;
        PutCheap = putCheap;
        PutExpensive = putExpensive;
        House = house;
        return this;
    }


    public bool CanExecute()
    {
        if (House.IsFirstPlayers != Player.FirstPlayer)
        {
            Game.Instance.DisplayMessage = "Can not put in refinement that is not yours";
            return false;
        }
        if (Player.Bag.GetCheapCrystalCount() < PutCheap)
        {
            Game.Instance.DisplayMessage = "You can't put more cheap crystals than you have in your backpack";

            return false;
        }
        if (Player.Bag.GetExpensiveCrystalCount() < PutExpensive)
        {
            Game.Instance.DisplayMessage = "You can't put more expensive crystals than you have in your backpack";
            return false;
        }
        if (!CanAct())
        {
            Game.Instance.DisplayMessage = "You must be near your refinement to put crystals in it.";
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
        Game.Instance.DisplayMessage = "Put to refinement Successful!";

        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }
}

