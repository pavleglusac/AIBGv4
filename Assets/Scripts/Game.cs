using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    public bool FirstPlayerTurn { get; set; } = true;
    public bool GameOver { get; set; } = false;
    public int TurnCount { get; set; } = 0;
    public string Winner { get; set; } = "";

    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public Board Board {get; set;}

    public CommandManager CommandManager { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

}
