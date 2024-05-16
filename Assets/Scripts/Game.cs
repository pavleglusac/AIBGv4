using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

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

    static ScriptRunner player1ScriptRunner = null;
    static ScriptRunner player2ScriptRunner = null;

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

        UnityEngine.Debug.Log("Game setup complete");
        UnityEngine.Debug.Log($"Player 1 script path: {player1ScriptPath}");
        UnityEngine.Debug.Log($"Player 2 script path: {player2ScriptPath}");

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
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        UnityEngine.Debug.Log("Setting up game! Timestamp: " + timestamp);
    }

    private void OnApplicationQuit()
    {
        KillSubprocesses();
    }

    public static void KillSubprocesses()
    {
        try
        {
            UnityEngine.Debug.LogError("Ubijam 1!");
            // KillProcessGroup(Game.Instance.player1ScriptRunner.process.Id);
            Game.player1ScriptRunner.process.Kill();
            UnityEngine.Debug.LogError("Ubiven 1!");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.ToString());
            try
            {
                UnityEngine.Debug.LogError("Ubijam 1 A!");
                // KillProcessGroup(player1ScriptRunner.process.Id);
                player1ScriptRunner.process.Kill();
                UnityEngine.Debug.LogError("Ubiven 1 A!");
            }
            catch (Exception e2)
            {
                UnityEngine.Debug.LogError("Failed to kill process 1");
            }
        }

        try
        {
            UnityEngine.Debug.LogError("Ubijam 2!");
            // KillProcessGroup(Game.Instance.player2ScriptRunner.process.Id);
            Game.player2ScriptRunner.process.Kill();
            UnityEngine.Debug.LogError("Ubiven 2!");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.ToString());
            try
            {
                UnityEngine.Debug.LogError("Ubijam 2 A!");
                // KillProcessGroup(player2ScriptRunner.process.Id);
                player2ScriptRunner.process.Kill();
                UnityEngine.Debug.LogError("Ubiven 2 A!");
            }
            catch (Exception e2)
            {
                UnityEngine.Debug.LogError("Failed to kill process 2");
            }
        }
    }

    public void ResetGame()
    {
        KillSubprocesses();
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
        UnityEngine.Debug.Log($"Game Over! {gameOverMessage}");
        UnityEngine.Debug.Log($"Game State: {Instance.GetGameState().Replace("\n", "").Replace("\t", "")}");
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
        DecreasePlayerStatuses();
        bool previousTurnFirstPlayer = Game.Instance.FirstPlayerTurn;
        if (!GetAlternatePlayer().IsFrozen())
            Game.Instance.FirstPlayerTurn = !Game.Instance.FirstPlayerTurn;
        UpdateAllPlayerStats(previousTurnFirstPlayer);
        UnityEngine.Debug.Log($"Switching Player! Current Player: {(FirstPlayerTurn ? 1 : 2)}");
        InvokeScript(FirstPlayerTurn);
    }

    public void InvokeScript(bool FirstPlayerTurn)
    {
        if (Game.Instance.GameOver || Game.Instance.Board.Pillars == null)
        {
            return;
        }

        ScriptRunner targetRunner = FirstPlayerTurn ? player1ScriptRunner : player2ScriptRunner;
        UnityEngine.Debug.Log($"Invoking script for player {(FirstPlayerTurn ? 1 : 2)}. Target Runner is not null: {targetRunner != null}");
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
            UnityEngine.Debug.Log("Script task finished!");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Script task failed!");
            UnityEngine.Debug.LogError(ex.ToString());
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
        // add turn count into json
        info += "\"turn\":" + TurnCount + ",";
        info += "\"firstPlayerTurn\":" + FirstPlayerTurn.ToString().ToLower() + ",";
        info += Player1.GetStats(true) + ",";
        info += Player2.GetStats(false) + ",";
        info += Board.DrawBoard();
        info += "}";
        info.Replace("\n", "").Replace("\t", "");
        return info;

    }
}
