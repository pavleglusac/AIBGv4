using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversionCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }

    public int XPCheap { get; set; }
    public int XPExpensive { get; set; }
    public int CoinsCheap { get; set; }
    public int CoinsExpensive { get; set; }
    public int EnergyCheap { get; set; }
    public int EnergyExpensive { get; set; }


    public int CheapTotal { get; set; }
    public int ExpensiveTotal { get; set; }
    public int CoinsTotal { get; set; }
    public int XPTotal { get; set; }
    public int EnergyTotal { get; set; }


    public bool isDone { get; set; } = false;

    public ConversionCommand Initialize(Player player, int XPCheap, int XPExpensive, int coinsCheap, int coinsExpensive, int energyCheap, int energyExpensive)
    {
        int XPCheapConversionRate = int.Parse(PlayerPrefs.GetString("cheap_to_xp"));
        int XPExpensiveConversionRate = int.Parse(PlayerPrefs.GetString("exp_to_xp"));
        int coinsCheapConversionRate = int.Parse(PlayerPrefs.GetString("cheap_to_coins"));
        int coinsExpensiveConversionRate = int.Parse(PlayerPrefs.GetString("exp_to_coins"));
        int energyCheapConversionRate = int.Parse(PlayerPrefs.GetString("cheap_to_energy"));
        int energyExpensiveConversionRate = int.Parse(PlayerPrefs.GetString("exp_to_energy"));

        XPTotal = XPCheap * XPCheapConversionRate + XPExpensive * XPExpensiveConversionRate;
        CoinsTotal = coinsCheap * coinsCheapConversionRate + coinsExpensive * coinsExpensiveConversionRate;
        EnergyTotal = energyCheap * energyCheapConversionRate + energyExpensive * energyExpensiveConversionRate;

        Player = player;
        this.XPCheap = XPCheap;
        this.XPExpensive = XPExpensive;
        CoinsCheap = coinsCheap;
        CoinsExpensive = coinsExpensive;
        EnergyCheap = energyCheap;
        EnergyExpensive = energyExpensive;
        CheapTotal = XPCheap + CoinsCheap + EnergyCheap;
        ExpensiveTotal = XPExpensive + CoinsExpensive + EnergyExpensive;
        return this;
    }

    public void Execute()
    {

        for (int i = 0; i < CheapTotal; i++)
        {
            Player.Bag.PopCheapCrystal();
        }
        for (int i = 0; i < ExpensiveTotal; i++)
        {
            Player.Bag.PopExpensiveCrystal();
        }
        Player.AddCoins(CoinsTotal);
        Player.AddXP(XPTotal);
        Player.IncreaseEnergy(EnergyTotal);
        Game.Instance.DisplayMessage = "Conversion successful!";

        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }

    public bool CanExecute()
    {
        if (!Player.FirstPlayer)
        {
            if (Player.X != 0 || Player.Z != 9)
            {
                Game.Instance.DisplayMessage = "You need to be on the base to convert minerals and diamonds";
                return false;
            }
        }
        else
        {
            if (Player.X != 9 || Player.Z != 0)
            {
                Game.Instance.DisplayMessage = "You need to be on the base to convert minerals and diamonds";
                return false;
            }
        }

        if (Player.Bag.GetCheapCrystalCount() < CheapTotal)
        {
            Game.Instance.DisplayMessage = "You can't put more minerals than you have in your backpack";
            return false;
        }
        if (Player.Bag.GetExpensiveCrystalCount() < ExpensiveTotal)
        {
            Game.Instance.DisplayMessage = "You can't put more diamonds than you have in your backpack";
            return false;
        }
        if (CheapTotal == 0 && ExpensiveTotal == 0)
        {
            Game.Instance.DisplayMessage = "You need to convert at least one mineral or diamond";
            return false;
        }
        return true;
    }
}
