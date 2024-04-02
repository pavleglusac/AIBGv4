using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MineCommand : MonoBehaviour, IEnergySpendingCommand
{
    public Player Player { get; set; }
    public bool isDone { get; set; } = false;
    public bool isMining { get; set; } = false;
    private bool isCoroutineRunning = false;
    private bool isCheapCrystal = true;
    Crystal Crystal { get; set; }
    private int crystalWeight;
    public Pillar Pillar { get; set; }
    private int x;
    private int z;

    // public MineCommand Initialize(Player player, bool isCrystal1)
    // {
    //     this.Player = player;
    //     this.isCheapCrystal = isCrystal1;
    //     return this;
    // }

    public MineCommand Initialize(Player player, int x, int z)
    {
        this.Player = player;
        Pillar pillar = Game.Instance.Board.Pillars[x, z];
        this.Pillar = pillar;
        this.isCheapCrystal = pillar.PillarState == PillarState.CheapCrystal;
        this.x = x;
        this.z = z;
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
        isDone = true;
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
    }

    private IEnumerator Mine()
    {
        Animator animator = Crystal.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
            animator.speed = 4.0f;
            string trigger = isCheapCrystal ? "ShakeCrystal1Trigger" : "ShakeCrystal2Trigger";
            animator.SetTrigger(trigger);
        }

        Crystal.RemainingMineHits--;
        if (Crystal.RemainingMineHits == 0)
        {
            Crystal.IsEmpty = true;
            Crystal.TurnInWhichCrystalBecameEmpty = Game.Instance.TurnCount;
        }

        if (isCheapCrystal)
        {
            Player.Bag.AddCheapCrystal();
            Game.Instance.DisplayMessage = "Mineral is mined";
        }
        else
        {
            Game.Instance.DisplayMessage = "Diamond is mined";
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
        if (!CanAct())
        {
            Game.Instance.DisplayMessage = "You are not close enough to mine";
            return false;
        }

        CheapCrystal cheapCrystal = Game.Instance.Board.CheapCrystals.FirstOrDefault(c => c.X == x && c.Z == z);
        if (cheapCrystal != null)
        {
            this.Crystal = cheapCrystal;
            this.crystalWeight = CheapCrystal.GetUnprocessedWeight();
        }
        else
        {
            ExpensiveCrystal expensiveCrystal = Game.Instance.Board.ExpensiveCrystals.FirstOrDefault(c => c.X == x && c.Z == z);
            if (expensiveCrystal != null)
            {
                this.Crystal = expensiveCrystal;
                this.crystalWeight = ExpensiveCrystal.GetUnprocessedWeight();
            }
            else
            {
                Game.Instance.DisplayMessage = "No crystal found";
                return false;
            }
        }

        if (Crystal.RemainingMineHits == 0 && Crystal.TurnInWhichCrystalBecameEmpty == -1)
        {
            Game.Instance.DisplayMessage = "Crystal is not replenished";
            Crystal.TurnInWhichCrystalBecameEmpty = Game.Instance.TurnCount;
            Crystal.IsEmpty = true;
            return false;
        }
        if ((Game.Instance.TurnCount > Crystal.TurnInWhichCrystalBecameEmpty + Crystal.ReplenishTurns) && Crystal.RemainingMineHits == 0)
        {
            Crystal.RemainingMineHits = Crystal.MaxMineHits;
            Crystal.TurnInWhichCrystalBecameEmpty = -1;
            Crystal.IsEmpty = false;
        }
        if (Crystal.RemainingMineHits == 0)
        {
            Game.Instance.DisplayMessage = "Crystal is not replenished";
            return false;
        }
        if (Player.Energy < GetEnergyCost())
        {
            Game.Instance.DisplayMessage = "Not enough energy for mining";
            return false;
        }
        if (Player.Bag.GetRemainingCapacity() < crystalWeight)
        {
            Game.Instance.DisplayMessage = "Not enough space in the bag";
            return false;
        }
        return true;
    }

    public int GetEnergyCost()
    {
        return isCheapCrystal
            ? int.Parse(PlayerPrefs.GetString("mining_energy_cheap_crystal_loss"))
            : int.Parse(PlayerPrefs.GetString("mining_energy_expensive_crystal_loss"));
    }

    bool CanAct()
    {
        List<Pillar> neighbours = Game.Instance.Board.getNeighbours(Pillar);
        if (neighbours.Contains(Player.Position))
        {
            return true;
        }
        return false;
    }
}