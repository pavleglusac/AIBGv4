using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapCrystal : MonoBehaviour
{
    public GameObject Crystal1ParentObject { get; set; }
    public GameObject Crystal1Object { get; set; }
    public Pillar Position { get; set; }
    public int X { get; set; }
    public int Z { get; set; }
    // Start is called before the first frame update

    public int MaxMineHits { get; set; }
    public int RemainingMineHits { get; set; }
    public int ReplenishTurns { get; set; }
    public int TurnInWhichCrystalBecameEmpty { get; set; } = -1;

    void Start()
    {
        MaxMineHits = int.Parse(PlayerPrefs.GetString("cheap_crystal_mine_hits"));
        RemainingMineHits = MaxMineHits;
        ReplenishTurns = int.Parse(PlayerPrefs.GetString("cheap_crystal_replenish_turns"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when the player is clicked
    void OnMouseDown()
    {

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

        Debug.Log("Crystal is mined, remaining hits: " + RemainingMineHits);
        if (!CanAnimate())
        {
            return;
        }
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
            animator.speed = 4.0f;
            animator.SetTrigger("ShakeCrystal1Trigger");
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
