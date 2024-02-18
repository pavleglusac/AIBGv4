using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversionCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }

    public int CheapTotal { get; set; }
    public int ExpensiveTotal { get; set; }

    public int CoinsTotal { get; set; }
    public int XPTotal { get; set; }
    public int EnergyTotal { get; set; }


    private bool isDone = false;

    public ConversionCommand Initialize(Player player, int cheapTotal, int expensiveTotal, int XPTotal, int coinsTotal, int energyTotal)
    {
        Player = player;
        CheapTotal = cheapTotal;
        ExpensiveTotal = expensiveTotal;
        this.XPTotal = XPTotal;
        CoinsTotal = coinsTotal;
        EnergyTotal = energyTotal;
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
