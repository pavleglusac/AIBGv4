using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public class House : MonoBehaviour
{


    public GameObject HouseParentObject { get; set; }
    public GameObject HouseObject { get; set; }
    public Pillar Position { get; set; }
    public bool IsFirstPlayers { get; set; }
    public int X { get; set; }
    public int Z { get; set; }
    public int Health { get; set; }
    public List<Tuple<CheapCrystalItem, int>> CheapCrystals { get; set; } = new List<Tuple<CheapCrystalItem, int>>();
    public List<Tuple<ExpensiveCrystalItem, int>> ExpensiveCrystals { get; set; } = new List<Tuple<ExpensiveCrystalItem, int>>();


    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = HouseParentObject.transform.position;
    }

    void Update()
    {
        int unprocessedCheapCrystalCount = GetUnprocessedCheapCrystalCount();
        int unprocessedExpensiveCrystalCount = GetUnprocessedExpensiveCrystalCount();
        int processedCheapCrystalCount = GetProcessedCheapCrystalCount();
        int processedExpensiveCrystalCount = GetProcessedExpensiveCrystalCount();
        var position = initialPosition;

        if (unprocessedCheapCrystalCount > 0 || unprocessedExpensiveCrystalCount > 0)
        {
            float time = Time.time;
            float x = initialPosition.x + Mathf.Sin(time * 3) * 0.05f;
            float z = initialPosition.z + Mathf.Cos(time * 10 + 1) * 0.05f;
            position = new Vector3(x, position.y, z);
        }

        if (processedCheapCrystalCount > 0 || processedExpensiveCrystalCount > 0)
        {
            float time = Time.time;
            float y = initialPosition.y + Mathf.Abs(Mathf.Sin(time * 2)) * 0.25f;
            position = new Vector3(position.x, y, position.z);
        }

        if (processedCheapCrystalCount == 0 && processedExpensiveCrystalCount == 0 && unprocessedCheapCrystalCount == 0 && unprocessedExpensiveCrystalCount == 0)
        {
            position = initialPosition;
        }
        HouseParentObject.transform.position = position;

        Player player = null;
        if (IsFirstPlayers)
        {
            player = Game.Instance.Player1;
        }
        else
        {
            player = Game.Instance.Player2;
        }
        if ((Mathf.Abs(player.X - this.X) <= 1 && Mathf.Abs(player.Z - this.Z) <= 1))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }


    }

    public void Destroy()
    {
        Position.LastState = Position.PillarState;
        Position.PillarState = PillarState.Empty;
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        // Added so that players can not click 3D objects trough UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (Game.Instance.GetCurrentPlayer().FirstPlayer != this.IsFirstPlayers)
        {
            // TODO: Attack
            Game.Instance.selectedHouse = this;
            Debug.Log("Attack");
            Debug.Log(this.Health);
            Actions.AttackHouse(X, Z);
        }
        else
        {
            Game.Instance.selectedHouse = this;
            RefinementMenuNavigation.Instance.OpenRefinementMenu(this);
        }

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Actions.BuildHouse(this.X, this.Z);
        }
    }

    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }

    public int GetProcessedCheapCrystalCount()
    {
        int cheapCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("cheap_crystal_processing_turns"));
        return CheapCrystals.Count(crystal => Game.Instance.TurnCount - crystal.Item2 >= cheapCrystalProcessingTurns);
    }

    public int GetUnprocessedCheapCrystalCount()
    {
        int cheapCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("cheap_crystal_processing_turns"));
        return CheapCrystals.Count(crystal => Game.Instance.TurnCount - crystal.Item2 < cheapCrystalProcessingTurns);
    }

    public int GetProcessedExpensiveCrystalCount()
    {
        int expensiveCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("expensive_crystal_processing_turns"));
        return ExpensiveCrystals.Count(crystal => Game.Instance.TurnCount - crystal.Item2 >= expensiveCrystalProcessingTurns);
    }

    public int GetUnprocessedExpensiveCrystalCount()
    {
        int expensiveCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("expensive_crystal_processing_turns"));
        return ExpensiveCrystals.Count(crystal => Game.Instance.TurnCount - crystal.Item2 < expensiveCrystalProcessingTurns);
    }

    public CheapCrystalItem PopProcessedCheapCrystal()
    {
        int cheapCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("cheap_crystal_processing_turns"));
        Tuple<CheapCrystalItem, int> crystal = CheapCrystals.First(c => Game.Instance.TurnCount - c.Item2 >= cheapCrystalProcessingTurns);
        CheapCrystals.Remove(crystal);
        crystal.Item1.isProcessed = true;
        return crystal.Item1;
    }

    public ExpensiveCrystalItem PopProcessedExpensiveCrystal()
    {
        int expensiveCrystalProcessingTurns = int.Parse(PlayerPrefs.GetString("expensive_crystal_processing_turns"));
        Tuple<ExpensiveCrystalItem, int> crystal = ExpensiveCrystals.First(c => Game.Instance.TurnCount - c.Item2 >= expensiveCrystalProcessingTurns);
        ExpensiveCrystals.Remove(crystal);
        crystal.Item1.isProcessed = true;
        return crystal.Item1;
    }






}
