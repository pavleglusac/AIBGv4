using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;

public class Player
{
    public string Name { get; set; }
    public int Energy { get; set; }
    public int Xp { get; set; }
    public int Coins { get; set; }
    public int[] Position { get; set; }
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
    public char[,] Grid { get; set; }

    public override string ToString()
    {
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
}

public class GameLogic
{
    public static List<Move> FindDiamonds(Board board, Player player)
    {
        int minDist = 1000;
        List<Move> targetPos = new List<Move>();

        // Find all diamond positions on the board
        for (int i = 0; i < board.Grid.GetLength(0); i++)
        {
            for (int j = 0; j < board.Grid.GetLength(1); j++)
            {
                if (board.Grid[i, j] == 'D')
                {
                    targetPos.Add(new Move(i, j));
                }
            }
        }

        List<Move> closestPath = null;
        // Try to find a path to each diamond
        foreach (var pos in targetPos)
        {
            List<Move> path = FindPathForTarget(board, player, pos);
            if (path != null && path.Count < minDist)
            {
                minDist = path.Count;
                closestPath = path;
            }
        }
        return closestPath;
    }

    public static List<Move> FindPathForTarget(Board board, Player player, Move targetPos)
    {
        int[] myPos = player.Position;
        Queue<Tuple<int, int, List<Move>>> queue = new Queue<Tuple<int, int, List<Move>>>();
        HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>>();

        queue.Enqueue(Tuple.Create(myPos[0], myPos[1], new List<Move>()));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int x = current.Item1;
            int y = current.Item2;
            List<Move> path = current.Item3;

            if (x == targetPos.X && y == targetPos.Y)
            {
                return path;
            }

            if (visited.Contains(Tuple.Create(x, y)) || board.Grid[x, y] != 'E')
            {
                continue;
            }

            visited.Add(Tuple.Create(x, y));

            foreach (var dir in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
            {
                int newX = x + dir.Item1;
                int newY = y + dir.Item2;

                if (newX >= 0 && newX < board.Grid.GetLength(0) && newY >= 0 && newY < board.Grid.GetLength(1))
                {
                    List<Move> newPath = new List<Move>(path);
                    newPath.Add(new Move(newX, newY));
                    queue.Enqueue(Tuple.Create(newX, newY, newPath));
                }
            }
        }

        return null; // No path found
    }

    public static Move NextMove(List<Move> path, int[] myPos)
    {
        if (path == null || path.Count == 0)
            return null;

        int[] directionFirstStep = new int[] { path[0].X - myPos[0], path[0].Y - myPos[1] };
        if (path.Count == 1)
            return path[0];

        for (int i = 1; i < path.Count; i++)
        {
            int[] directionNextStep = new int[] { path[i].X - path[i - 1].X, path[i].Y - path[i - 1].Y };
            if (directionNextStep[0] != directionFirstStep[0] || directionNextStep[1] != directionFirstStep[1])
                return path[i - 1];
        }

        return path[path.Count - 1];
    }

    public static bool DiamondsInBag(Player player)
    {
        return player.RawDiamonds > 0;
    }

    public static int[] NextToType(Board board, Player player, char type = 'D')
    {
        int[] myPos = player.Position;
        int myX = myPos[0], myY = myPos[1];
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // Skip current position
                if (i == 0 && j == 0)
                    continue;
                // Check bounds
                if (!(myX + i >= 0 && myX + i < board.Grid.GetLength(0) && myY + j >= 0 && myY + j < board.Grid.GetLength(1)))
                    continue;
                if (board.Grid[myX + i, myY + j] == type)
                    return new int[] { myX + i, myY + j };
            }
        }
        return null;
    }

    public static bool OnType(Board board, Player player, char type = 'D')
    {
        int[] myPos = player.Position;
        return board.Grid[myPos[0], myPos[1]] == type;
    }

    public static void Act(string line)
    {
        GameState gameState = new GameState(line);

        Player player = gameState.Player2;
        Board board = gameState.Board;

        // If player has no diamonds in bag
        if (!DiamondsInBag(player))
        {
            int[] nextToDiamondsPos = NextToType(board, player);
            if (nextToDiamondsPos != null)
            {
                Console.WriteLine($"mine {nextToDiamondsPos[0]} {nextToDiamondsPos[1]}");
                return;
            }

            List<Move> closestDiamond = FindDiamonds(board, player);
            Move nextMoveToDiamond = NextMove(closestDiamond, player.Position);
            if (nextMoveToDiamond != null)
            {
                Console.WriteLine($"move {nextMoveToDiamond.X} {nextMoveToDiamond.Y}");
            }
            else
            {
                Console.WriteLine("rest");
            }
        }
        else
        {
            // Go to base at (0, 11)
            if (player.Position[0] == 0 && player.Position[1] == 11)
            {
                Console.WriteLine($"conv 0 diamond 0 mineral to coins, 0 diamond 0 mineral to energy, {player.RawDiamonds} diamond {player.RawMinerals} mineral to xp");
                return;
            }

            int[] nextToBase = NextToType(board, player, 'B');
            if (nextToBase != null)
            {
                Console.WriteLine($"move {nextToBase[0]} {nextToBase[1]}");
                return;
            }

            List<Move> closestBase = FindPathForTarget(board, player, new Move(0, 11));
            Move nextMoveToBase = NextMove(closestBase, player.Position);
            if (nextMoveToBase != null)
            {
                Console.WriteLine($"move {nextMoveToBase.X} {nextMoveToBase.Y}");
            }
            else
            {
                Console.WriteLine("rest");
            }
        }
    }

    public static void Main(string[] args)
    {
        while (true)
        {
            string line = Console.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                Act(line);
            }
            Thread.Sleep(2000); // Sleep for 2 seconds
        }
    }
}
