using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCommand : MonoBehaviour, IEnergySpendingCommand
{
    public Player Player { get; set; }
    public bool isDone { get; set; } = false;
    public bool isMining { get; set; } = false;
    private bool isCoroutineRunning = false;
    private bool isCheapCrystal = true;

    public MineCommand Initialize(Player player, bool isCrystal1)
    {
        this.Player = player;
        this.isCheapCrystal = isCrystal1;
        return this;
    }

    public void Execute()
    {
        isMining = true;
        if (isMining && !isCoroutineRunning)
        {
            isCoroutineRunning = true;
            StartCoroutine(ProcessMining());
        }
        Player.DecreaseEnergy(GetEnergyCost());
        Game.Instance.SwitchPlayersAndDecreaseStats();
    }

    public void Update()
    {

    }

    private IEnumerator ProcessMining()
    {
        isCoroutineRunning = true;
        yield return StartCoroutine(Mine());
        isMining = false;
        isCoroutineRunning = false;
        isDone = true;

    }

    private IEnumerator Mine()
    {
        if (isCheapCrystal)
        {
            Player.Bag.AddCheapCrystal();
        }
        else
        {
            Player.Bag.AddExpensiveCrystal();
        }

        yield return new WaitForSeconds(0.0f);
    }

    public bool IsDone()
    {
        return isDone;
    }

    public bool CanExecute()
    {
        return Player.Energy >= GetEnergyCost();
    }

    public int GetEnergyCost()
    {
        return isCheapCrystal
            ? int.Parse(PlayerPrefs.GetString("mining_energy_cheap_crystal_loss"))
            : int.Parse(PlayerPrefs.GetString("mining_energy_expensive_crystal_loss"));
    }
}