using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnergySpendingCommand : ICommand
{
    public int GetEnergyCost();

}