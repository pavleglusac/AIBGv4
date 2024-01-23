using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpensiveCrystal : MonoBehaviour
{
    public GameObject Crystal2ParentObject { get; set; }
    public GameObject Crystal2Object { get; set; }
    public Pillar Position { get; set; }
    public int X { get; set; }
    public int Z { get; set; }

    public int MaxMineHits { get; set; }
    public int RemainingMineHits { get; set; }
    public int ReplenishTurns { get; set; }
    public int TurnInWhichCrystalBecameEmpty { get; set; } = -1;


    // Start is called before the first frame update
    void Start()
    {
        MaxMineHits = int.Parse(PlayerPrefs.GetString("expensive_crystal_mine_hits"));
        RemainingMineHits = MaxMineHits;
        ReplenishTurns = int.Parse(PlayerPrefs.GetString("expensive_crystal_replenish_turns"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when the player is clicked
    void OnMouseDown()
    {
        Debug.Log(Game.Instance.TurnCount + " " + TurnInWhichCrystalBecameEmpty + " " + RemainingMineHits + " " + ReplenishTurns);
        if (RemainingMineHits == 0 && TurnInWhichCrystalBecameEmpty == -1)
        {
            Debug.Log("Crystal is empty");
            TurnInWhichCrystalBecameEmpty = Game.Instance.TurnCount;

        }


        if ((Game.Instance.TurnCount > TurnInWhichCrystalBecameEmpty + ReplenishTurns) && RemainingMineHits == 0)
        {
            Debug.Log("Crystal is replenished");
            RemainingMineHits = MaxMineHits;
            TurnInWhichCrystalBecameEmpty = -1;
        }



        if (RemainingMineHits == 0)
        {
            GameObject commandObject = new GameObject("NopCommandObject");
            NopCommand nopCommandInstance = commandObject.AddComponent<NopCommand>();
            nopCommandInstance.Initialize(Game.Instance.GetCurrentPlayer());
            Game.Instance.CommandManager.AddCommand(nopCommandInstance);

            Game.Instance.SwitchPlayersAndDecreaseStats();

            Game.Instance.TurnCount++;

            return;
        }

        RemainingMineHits--;


        if (!CanAnimate())
        {
            return;
        }
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
            animator.speed = 4.0f;
            animator.SetTrigger("ShakeCrystal2Trigger");
        }
        Position.Move();
    }


    bool CanAnimate()
    {
        List<Pillar> neighbours = Game.Instance.Board.getNeighbours(Position);
        if (neighbours.Contains(Game.Instance.GetCurrentPlayer().Position))
        {
            return true;
        }
        return false;
    }


    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }


}
