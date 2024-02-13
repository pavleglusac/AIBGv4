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

    public bool IsEmpty { get; set; }

    public Dictionary<string, Tuple<Material, Material>> materials = new Dictionary<string, Tuple<Material, Material>>();


    // Start is called before the first frame update
    void Start()
    {
        MaxMineHits = int.Parse(PlayerPrefs.GetString("expensive_crystal_mine_hits"));
        RemainingMineHits = MaxMineHits;
        ReplenishTurns = int.Parse(PlayerPrefs.GetString("expensive_crystal_replenish_turns"));

        foreach (Transform child in transform)
        {
            if (child.name == "Rock004") continue;
            if (!child.TryGetComponent<Renderer>(out var rend)) continue;
            materials[child.name] = new Tuple<Material, Material>(rend.materials[0], rend.materials[1]);
        }
        IsEmpty = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Game.Instance.TurnCount > TurnInWhichCrystalBecameEmpty + ReplenishTurns) && RemainingMineHits == 0 &&
            TurnInWhichCrystalBecameEmpty != -1)
        {
            IsEmpty = false;
        }

        foreach (Transform child in transform)
        {
            if (child.name == "Rock003") continue;
            if (!child.TryGetComponent<Renderer>(out var rend)) continue;
            Tuple<Material, Material> material = materials[child.name];
            if (IsEmpty)
            {
                rend.materials[0] = material.Item2;
                rend.materials[1] = material.Item2;
                rend.material = material.Item2;
            }
            else
            {
                rend.materials[0] = material.Item1;
                rend.materials[1] = material.Item1;
                rend.material = material.Item1;
            }
        }
    }

    // Called when the player is clicked
    void OnMouseDown()
    {
        if(!CanAct(Game.Instance.GetCurrentPlayer()))
        {
            Game.Instance.SwitchPlayersAndDecreaseStats();
            return;
        }

        if (RemainingMineHits == 0 && TurnInWhichCrystalBecameEmpty == -1)
        {
            Debug.Log("Crystal is empty");
            TurnInWhichCrystalBecameEmpty = Game.Instance.TurnCount;
            IsEmpty = true;

        }

        if ((Game.Instance.TurnCount > TurnInWhichCrystalBecameEmpty + ReplenishTurns) && RemainingMineHits == 0)
        {
            Debug.Log("Crystal is replenished");
            RemainingMineHits = MaxMineHits;
            TurnInWhichCrystalBecameEmpty = -1;
            IsEmpty = false;
        }

        if (RemainingMineHits == 0)
        {
            GameObject commandObject = new GameObject("NopCommandObject");
            NopCommand nopCommandInstance = commandObject.AddComponent<NopCommand>();
            nopCommandInstance.Initialize(Game.Instance.GetCurrentPlayer());
            Game.Instance.CommandManager.AddCommand(nopCommandInstance);
            Game.Instance.SwitchPlayersAndDecreaseStats();

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
        Actions.Mine(PillarState.ExpensiveCrystal, Game.Instance.GetCurrentPlayer());
        
    }

    bool CanAct(Player player)
    {
        List<Pillar> neighbours = Game.Instance.Board.getNeighbours(Position);
        if (neighbours.Contains(player.Position))
        {
            return true;
        }
        return false;
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
