using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public static bool IsPaused = false;
    public bool FirstPlayerTurn { get; set; } = true;
    public bool GameOver { get; set; } = false;
    public int TurnCount { get; set; } = 0;
    public string Winner { get; set; } = "";

    public Player Player1 { get; set; }
    public Player Player2 { get; set; }

    public Base BasePlayer1 { get; set; }
    public Base BasePlayer2 { get; set; }

    public Board Board { get; set; }

    [HideInInspector] public int rows;
    [HideInInspector] public int columns;
    public float spacing = 1.3f;
    public float animationDelay = 0.1f;
    [HideInInspector] public int numberOfCheapCrystalGroups;
    [HideInInspector] public int numberOfExpensiveCrystalGroups;
    [HideInInspector] public int numberOfCheapCrystalsInGroup;
    [HideInInspector] public int numberOfExpensiveCrystalsInGroup;

    [HideInInspector] public House selectedHouse;


    public CommandManager CommandManager { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            SetupGame();

        }
        else
        {

            Destroy(gameObject);
        }
    }

    public void SetupGame()
    {
        ResetGame();
        numberOfCheapCrystalGroups = int.Parse(PlayerPrefs.GetString("number_of_cheap_crystal_groups"));
        numberOfExpensiveCrystalGroups = int.Parse(PlayerPrefs.GetString("number_of_expensive_crystal_groups"));
        numberOfCheapCrystalsInGroup = int.Parse(PlayerPrefs.GetString("number_of_cheap_crystals_in_group"));
        numberOfExpensiveCrystalsInGroup = int.Parse(PlayerPrefs.GetString("number_of_expensive_crystals_in_group"));
        rows = int.Parse(PlayerPrefs.GetString("board_size"));
        columns = int.Parse(PlayerPrefs.GetString("board_size"));

        // create a new board but board is mono behaviour
        Board = new GameObject("Board").AddComponent<Board>();
        CommandManager = new GameObject("CommandManager").AddComponent<CommandManager>();
        DontDestroyOnLoad(gameObject);

    }

    public void ResetGame()
    {
        IsPaused = false;
        FirstPlayerTurn = true;
        Winner = "";
        TurnCount = 0;
        GameOver = false;
        selectedHouse = null;
        Instance = this;
        if (Player1 != null && Player2 != null)
        {
            Player2.SetupPlayer("Pupoljci");
            Player1.SetupPlayer("Crni Cerak");
        }
    }

    public void UpdateAllPlayerStats(String DisplayMessage)
    {
        PlayerStatsHandle.Instance.UpdateGUI(Player1, Player2, TurnCount, FirstPlayerTurn, DisplayMessage);
    }

    public static void PauseGame()
    {
        if (IsPaused)
            return;
        Time.timeScale = 0f;
        IsPaused = true;
    }
    public static void ResumeGame()
    {
        if (!IsPaused)
            return;
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public static void EndGame()
    {
        ResumeGame();
        SceneManager.LoadScene("Main menu");
    }


    public Player GetCurrentPlayer()
    {
        if (FirstPlayerTurn)
        {
            return Player1;
        }
        return Player2;
    }

    public Player GetAlternatePlayer()
    {
        if (FirstPlayerTurn)
        {
            return Player2;
        }
        return Player1;
    }

    public void SwitchPlayersAndDecreaseStats()
    {
        Game.Instance.TurnCount++;
        string DisplayMessage = "gas";
        if (!GetAlternatePlayer().IsFrozen())
            FirstPlayerTurn = !FirstPlayerTurn;
        DecreasePlayerStatuses();
        UpdateAllPlayerStats(DisplayMessage);
    }

    public void DecreasePlayerStatuses()
    {
        GetAlternatePlayer().DecreaseDazeTurns();
        GetAlternatePlayer().DecreaseFrozenTurns();
        GetCurrentPlayer().DecreaseIncreasedBackpackStorageTurns();

    }
}
