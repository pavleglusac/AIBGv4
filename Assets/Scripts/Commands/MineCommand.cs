using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }
    public bool isDone { get; set; } = false;
    public bool isMining { get; set; } = false;
    private bool isCoroutineRunning = false;
    private bool isCrystal1 = true;

    public MineCommand Initialize(Player player, bool isCrystal1)
    {
        this.Player = player;
        this.isCrystal1 = isCrystal1;
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
        if(isCrystal1)
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
}