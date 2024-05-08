using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using System.Text.RegularExpressions;

while (true)
{
    StreamReader sr = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    string line = "";
    while (true)
    {
        char c = (char)sr.Read();
        if (c == '\n')
        {
            break;
        }
        line += c;
    }

    if (!string.IsNullOrEmpty(line))
    {
        GameLogic.Act(line);
    }
    Thread.Sleep(2000); // Sleep for 2 seconds
}





public class Player
{
    public string? Name { get; set; }
    public int Energy { get; set; }
    public int Xp { get; set; }
    public int Coins { get; set; }
    public int[]? Position { get; set; }
    public int IncreasedBackpackDuration { get; set; }
    public int DazeTurns { get; set; }
    public int FrozenTurns { get; set; }
    public int BackpackCapacity { get; set; }
    public int RawMinerals { get; set; }
    public int ProcessedMinerals { get; set; }
    public int RawDiamonds { get; set; }
    public int ProcessedDiamonds { get; set; }


}

public class Board
{
    public char[,]? Grid { get; set; }

    public override string ToString()
    {
        if (Grid == null)
        {
            return "";
        }
        int rows = Grid.GetLength(0);
        int cols = Grid.GetLength(1);
        string result = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result += Grid[i, j];
            }
            result += "\n";
        }
        return result;
    }
}

public class GameState
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public Board Board { get; set; }

    public GameState(string text)
    {
        text = text.Replace("^", "\n");
        var dic = JsonConvert.DeserializeObject<dynamic>(text);
        if (dic == null)
        {
            throw new Exception("Invalid JSON");
        }
        Player1 = dic["player1"].ToObject<Player>();
        Player2 = dic["player2"].ToObject<Player>();
        Board = new Board { Grid = JsonConvert.DeserializeObject<char[,]>(dic["board"].ToString()) };
    }
}

public class Move
{
    public int X { get; set; }
    public int Y { get; set; }

    public Move(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}



public class GameLogic
{

    public static void Act(string line)
    {
        GameState gameState = new GameState(line);
        Board board = gameState.Board;


        Console.WriteLine("rest");

    }


}




