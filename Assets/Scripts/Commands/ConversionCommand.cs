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

    public int CoinsTotal { get; set; }
    public int XPTotal { get; set; }
    public int EnergyTotal { get; set; }


    private bool isDone = false;

    public ConversionCommand Initialize(Player player, int XPCheap, int XPExpensive, int coinsCheap, int coinsExpensive, int energyCheap, int energyExpensive, int XPTotal, int coinsTotal, int energyTotal)
    {
        Player = player;
        this.XPCheap = XPCheap;
        this.XPExpensive = XPExpensive;
        this.XPTotal = XPTotal;
        CoinsCheap = coinsCheap;
        CoinsExpensive = coinsExpensive;
        CoinsTotal = coinsTotal;
        EnergyCheap = energyCheap;
        EnergyExpensive = energyExpensive;
        EnergyTotal = energyTotal;
        return this;
    }

    public void Execute()
    {
        int cheapTotal = XPCheap + CoinsCheap + EnergyCheap;
        for (int i = 0; i < cheapTotal; i++)
        {
            Player.Bag.PopCheapCrystal();
        }
        int expensiveTotal = XPExpensive + CoinsExpensive + EnergyExpensive;
        for (int i = 0; i < expensiveTotal; i++)
        {
            Player.Bag.PopExpensiveCrystal();
        }
        Player.AddCoins(CoinsTotal);
        Player.AddXP(XPTotal);
        Player.IncreaseEnergy(EnergyTotal);
        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }

    public bool CanExecute()
    {
        return true;
    }
}
