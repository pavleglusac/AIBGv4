using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    [HideInInspector] public int totalCheapCrystalCount;
    [HideInInspector] public int totalExpensiveCrystalCount;
    [SerializeField] public Text player1Name;
    [SerializeField] public Text player2Name;
    [SerializeField] public Text player1Coins;
    [SerializeField] public Text player2Coins;
    [SerializeField] public Text player1Energy;
    [SerializeField] public Text player2Energy;
    [SerializeField] public Text player1XP;
    [SerializeField] public Text player2XP;

    public CommandManager CommandManager { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            totalCheapCrystalCount = int.Parse(PlayerPrefs.GetString("number_of_cheap_crystals"));
            totalExpensiveCrystalCount = int.Parse(PlayerPrefs.GetString("number_of_expensive_crystals"));
            rows = int.Parse(PlayerPrefs.GetString("board_size"));
            columns = int.Parse(PlayerPrefs.GetString("board_size"));
            // create a new board but board is mono behaviour
            Board = new GameObject("Board").AddComponent<Board>();
            CommandManager = new GameObject("CommandManager").AddComponent<CommandManager>();
            DontDestroyOnLoad(gameObject);


        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateAllPlayerStats()
    {

        player1Name.text = Player1.Name.ToString();
        player1Coins.text = Player1.Coins.ToString();
        player1Energy.text = Player1.Energy.ToString();
        player1XP.text = Player1.XP.ToString();

        player2Name.text = Player2.Name.ToString();
        player2Coins.text = Player2.Coins.ToString();
        player2Energy.text = Player2.Energy.ToString();
        player2XP.text = Player2.XP.ToString();
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

    public Player GetCurrentPlayer()
    {
        if (FirstPlayerTurn)
        {
            return Player1;
        }
        return Player2;
    }

}
