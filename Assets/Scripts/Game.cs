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
    public static Game Instance { get; set; }

    public static bool IsPaused = false;
    public bool FirstPlayerTurn { get; set; } = true;
    public bool GameOver { get; set; } = false;
    public int TurnCount { get; set; } = 0;
    public string Winner { get; set; } = "";
    public string DisplayMessage { get; set; } = "Good luck!";
    public bool ArePlayersLanded { get; set; } = false;

    public Player Player1 { get; set; }
    public Player Player2 { get; set; }

    public Base BasePlayer1 { get; set; }
    public Base BasePlayer2 { get; set; }

    public Board Board { get; set; }

    public LevelData LevelData { get; set; }

    [HideInInspector] public int rows;
    [HideInInspector] public int columns;
    public float spacing = 1.3f;
    public float animationDelay = 0.05f;
    [HideInInspector] public int numberOfCheapCrystalGroups;
    [HideInInspector] public int numberOfExpensiveCrystalGroups;
    [HideInInspector] public int numberOfCheapCrystalsInGroup;
    [HideInInspector] public int numberOfExpensiveCrystalsInGroup;

    [HideInInspector] public House selectedHouse;


    public CommandManager CommandManager { get; set; }
    public CommandParser CommandParser { get; set; }

    String player1ScriptPath;
    String player2ScriptPath;

    ScriptRunner player1ScriptRunner = null;
    ScriptRunner player2ScriptRunner = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        SetupGame();
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
        LevelData = new GameObject("LevelData").AddComponent<LevelData>();
        Board = new GameObject("Board").AddComponent<Board>();
        CommandManager = GameObject.FindObjectOfType<CommandManager>();
        if (CommandManager == null)
        {
            CommandManager = new GameObject("CommandManager").AddComponent<CommandManager>();
        }
        CommandParser = new GameObject("CommandParser").AddComponent<CommandParser>();
        CommandParser.CommandManager = CommandManager;
        CommandParser.Game = Instance;
        //DontDestroyOnLoad(gameObject);

        Debug.Log(player2ScriptPath);
        if (!string.IsNullOrEmpty(player1ScriptPath))
        {
            GameObject player1ScriptRunnerObject = new GameObject("ScriptRunnerObject");
            player1ScriptRunner = player1ScriptRunnerObject.AddComponent<ScriptRunner>();
            player1ScriptRunner.scriptPath = player1ScriptPath;
            player1ScriptRunner.CommandParser = CommandParser;
            player1ScriptRunner.StartProcess(player1ScriptRunner.scriptPath);
        }

        if (!string.IsNullOrEmpty(player2ScriptPath))
        {
            GameObject player2ScriptRunnerObject = new GameObject("ScriptRunnerObject");
            player2ScriptRunner = player2ScriptRunnerObject.AddComponent<ScriptRunner>();
            player2ScriptRunner.scriptPath = player2ScriptPath;
            player2ScriptRunner.CommandParser = CommandParser;
            player2ScriptRunner.StartProcess(player2ScriptRunner.scriptPath);
        }

    }


    public void ResetGame()
    {
        StateCallback.entered = 0;
        DisplayMessage = "Good Luck!";
        IsPaused = false;
        FirstPlayerTurn = true;
        Winner = "";
        TurnCount = 0;
        GameOver = false;
        selectedHouse = null;
        if (Player1 != null && Player2 != null)
        {
            Player2.SetupPlayer(PlayerPrefs.GetString("player2_name"));
            Player1.SetupPlayer(PlayerPrefs.GetString("player1_name"));
        }
        try {
            player1ScriptRunner?.process?.Kill();
        } catch (Exception e) {
        }

        try {
            player2ScriptRunner?.process?.Kill();
        } catch (Exception e) {
        }

        player1ScriptPath = PlayerPrefs.GetString("player_1_script_path");
        player2ScriptPath = PlayerPrefs.GetString("player_2_script_path");
        player1ScriptRunner = null;
        player2ScriptRunner = null;
        Instance = this;
    }

    public void UpdateAllPlayerStats(bool previousTurnFirstPlayer)
    {
        PlayerStatsHandle.Instance.UpdateGUI(Player1, Player2, Instance.TurnCount, FirstPlayerTurn, DisplayMessage, previousTurnFirstPlayer);
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

    public static void EndGame(string gameOverMessage)
    {
        PauseGame();
        PlayerStatsHandle.Instance.GameOverScreen(gameOverMessage);
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

    public void InvokeScript(bool FirstPlayerTurn)
    {
        if (Game.Instance.GameOver || Game.Instance.Board.Pillars == null) {
            return;
        }

        ScriptRunner targetRunner = FirstPlayerTurn ? player1ScriptRunner : player2ScriptRunner;
        Debug.Log($"Target runner jeeeee {targetRunner != null}");
        if (targetRunner == null)
        {
            return;
        }

        string msg = GetGameState();
        string currentPlayerName = FirstPlayerTurn ? Player1.Name : Player2.Name;
        RunWriteToProcessAsync(msg, targetRunner, currentPlayerName);
    }

    public async void RunWriteToProcessAsync(string msg, ScriptRunner targetRunner, string currentPlayerName)
    {
        try
        {
            await Task.Run(() => targetRunner.WriteToProcessAsync(msg, currentPlayerName));
            UnityEngine.Debug.Log("WriteToProcessAsync completed successfully.");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Task failed!");
            UnityEngine.Debug.LogError(ex.ToString());
            Game.Instance.DisplayMessage = ex.Message;
            Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
            Game.Instance.SwitchPlayersAndDecreaseStats();
        }
    }


    public void DecreasePlayerStatuses()
    {
        GetAlternatePlayer().DecreaseDazeTurns();
        GetAlternatePlayer().DecreaseFrozenTurns();
        GetCurrentPlayer().DecreaseIncreasedBackpackStorageTurns();

    }

    public string GetGameState()
    {
      
        string info = "{";
        info += Player1.GetStats(true) + ",";
        info += Player2.GetStats(false) + ",";
        info += Board.DrawBoard();
        info += "}";
        return info;

    }
}
