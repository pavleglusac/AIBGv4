using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public static bool IsPaused = false;
    public bool FirstPlayerTurn { get; set; } = true;
    public bool GameOver { get; set; } = false;
    public int TurnCount { get; set; } = 0;
    public string Winner { get; set; } = "";
    public string DisplayMessage { get; set; } = "Good luck!";

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
    public CommandParser CommandParser { get; set; }

    String player1ScriptPath;
    String player2ScriptPath;

    ScriptRunner player1ScriptRunner;
    ScriptRunner player2ScriptRunner;

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
        player1ScriptPath = PlayerPrefs.GetString("player_1_script_path");
        player2ScriptPath = PlayerPrefs.GetString("player_2_script_path");


        // create a new board but board is mono behaviour
        Board = new GameObject("Board").AddComponent<Board>();
        CommandManager = new GameObject("CommandManager").AddComponent<CommandManager>();
        CommandParser = new GameObject("CommandParser").AddComponent<CommandParser>();
        CommandParser.CommandManager = CommandManager;
        CommandParser.Game = Instance;
        DontDestroyOnLoad(gameObject);

        if (player1ScriptPath != null) {
            GameObject player1ScriptRunnerObject = new GameObject("ScriptRunnerObject");
            player1ScriptRunner = player1ScriptRunnerObject.AddComponent<ScriptRunner>();
            player1ScriptRunner.scriptPath = player1ScriptPath;
            player1ScriptRunner.CommandParser = CommandParser;
            player1ScriptRunner.StartProcess(player1ScriptRunner.scriptPath);
        }

        if (player2ScriptPath != null) {
            GameObject player2ScriptRunnerObject = new GameObject("ScriptRunnerObject");
            player2ScriptRunner = player2ScriptRunnerObject.AddComponent<ScriptRunner>();
            player2ScriptRunner.scriptPath = player2ScriptPath;
            player2ScriptRunner.CommandParser = CommandParser;
            player2ScriptRunner.StartProcess(player2ScriptRunner.scriptPath);
        }

    }

    public void ResetGame()
    {
        DisplayMessage = "Good Luck!";
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

    public void UpdateAllPlayerStats(bool previousTurnFirstPlayer)
    {
        PlayerStatsHandle.Instance.UpdateGUI(Player1, Player2, TurnCount, FirstPlayerTurn, DisplayMessage, previousTurnFirstPlayer);
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
        PauseGame();
        PlayerStatsHandle.Instance.GameOverScreen();
    }


    public Player GetCurrentPlayer()
    {
        if (Game.Instance.FirstPlayerTurn)
        {
            return Game.Instance.Player1;
        }
        return Game.Instance.Player2;
    }

    public Player GetAlternatePlayer()
    {
        if (Game.Instance.FirstPlayerTurn)
        {
            return Player2;
        }
        return Player1;
    }

    public void SwitchPlayersAndDecreaseStats()
    {
        Game.Instance.TurnCount++;
        bool previousTurnFirstPlayer = Game.Instance.FirstPlayerTurn;
        if (!GetAlternatePlayer().IsFrozen())
            Game.Instance.FirstPlayerTurn = !Game.Instance.FirstPlayerTurn;
        DecreasePlayerStatuses();
        UpdateAllPlayerStats(previousTurnFirstPlayer);
        Debug.Log($"Switch Player! Current Player: {(FirstPlayerTurn ? 1 : 2)}");
        InvokeScript(FirstPlayerTurn);
    }

    public void InvokeScript(bool FirstPlayerTurn) {
        ScriptRunner targetRunner = FirstPlayerTurn ? player1ScriptRunner : player2ScriptRunner;
        if (targetRunner == null) {
            return;
        }
        string msg = GetGameState();

        Task.Run(() => targetRunner.WriteToProcessAsync(msg)).ContinueWith(task => 
        {
            if (task.IsFaulted)
            {
                UnityEngine.Debug.LogError(task.Exception?.ToString());
            }
            else
            {
                UnityEngine.Debug.Log("WriteToProcessAsync completed successfully.");
            }
        });

    }

    public void DecreasePlayerStatuses()
    {
        GetAlternatePlayer().DecreaseDazeTurns();
        GetAlternatePlayer().DecreaseFrozenTurns();
        GetCurrentPlayer().DecreaseIncreasedBackpackStorageTurns();

    }

    public string GetGameState()
    {
        string info = "";
        info += Player1.GetStats();
        info += Player2.GetStats();
        info += $@"
Refinement facility cost: {PlayerPrefs.GetString("refinement_facility_cost")}
Daze cost: (Duration: {PlayerPrefs.GetString("number_of_daze_turns")} turns): {PlayerPrefs.GetString("daze_cost")}
Freeze cost: (Duration: {PlayerPrefs.GetString("number_of_frozen_turns")} turns): {PlayerPrefs.GetString("freeze_cost")}
Backpack increase cost: (Duration: {PlayerPrefs.GetString("number_of_bigger_backpack_turns")} turns): {PlayerPrefs.GetString("bigger_backpack_cost")}
";

        info += Board.DrawBoard();
        info += $"Turn number: {TurnCount} / {PlayerPrefs.GetString("max_number_of_turns")}\n";
        info += $"Now playing: {GetCurrentPlayer().Name}";
        return info;

    }
}
