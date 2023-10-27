using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{

    public GameObject PillarObject { get; set; }

    public PillarState PillarState { get; set; } = PillarState.Empty;

    public int X { get; set; }
    public int Z { get; set; }

    List<Pillar> path;
    // keep track of original color
    List<Color> originalColors = new List<Color>();

    private int GetDirection(Pillar prev, Pillar current)
    {
        if (prev.X == current.X)
        {
            return prev.Z > current.Z ? 0 : 2;
        }
        else if (prev.Z == current.Z)
        {
            return prev.X > current.X ? 1 : 3;
        }
        return -1;
    }

    void OnMouseDown()
    {
        if (path == null || path.Count == 0)
        {
            return;
        }

        Pillar prev = path[0];
        int count = 1;
        int direction = -1;
        List<(int, int)> directions = new List<(int, int)>();

        for (int i = 1; i < path.Count; i++)
        {
            Pillar currentPillar = path[i];
            int newDirection = GetDirection(prev, currentPillar);
            
            if (direction == newDirection)
            {
                count++;
            }
            else
            {
                if (direction != -1)
                {
                    directions.Add((direction, count));
                }
                count = 1;
                direction = newDirection;
            }

            prev = currentPillar;
        }

        directions.Add((direction, count));

        // attach movementmanager to player if it doesn't exist
        // Player player = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1 : Game.Instance.Player2;
        // if (player.PlayerObject.GetComponent<MovementManager>() == null)
        // {
        //     player.PlayerObject.AddComponent<MovementManager>();
        //     player.PlayerObject.GetComponent<MovementManager>().player = player.PlayerObject;
        // }
        Player player = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1 : Game.Instance.Player2;
        for (int i = 0; i < directions.Count; i++)
        {
            // create commands
            GameObject commandObject = new GameObject("MoveCommandObject");
            MoveCommand moveCommandInstance = commandObject.AddComponent<MoveCommand>();
            moveCommandInstance.Initialize(player, directions[i].Item1, directions[i].Item2);
            Game.Instance.CommandManager.AddCommand(moveCommandInstance);
            // (int, int) dir = directions[i];
            // player.PlayerObject.GetComponent<MovementManager>().AddMovement(dir.Item1, dir.Item2);
        }

        // swap player turn
        Game.Instance.FirstPlayerTurn = !Game.Instance.FirstPlayerTurn;
        player.X = X;
        player.Z = Z;
    }

    void OnMouseEnter()
    {
        Pillar to = this;
        Pillar from;
        Color color;

        if (Game.Instance.FirstPlayerTurn)
        {
            from = Game.Instance.Board.Pillars[Game.Instance.Player1.X, Game.Instance.Player1.Z];
            color = Color.blue;
        }
        else
        {
            from = Game.Instance.Board.Pillars[Game.Instance.Player2.X, Game.Instance.Player2.Z];
            color = Color.red;
        }

        path = Algorithms.findPath(Game.Instance.Board, from, to);
        foreach (Pillar pillar in path)
        {
            originalColors.Add(pillar.PillarObject.GetComponent<Renderer>().material.color);
            pillar.PillarObject.GetComponent<Renderer>().material.color = color;
        }
    }

    void OnMouseExit()
    {
        if (path == null)
        {
            return;
        }

        foreach (Pillar pillar in path)
        {
            pillar.PillarObject.GetComponent<Renderer>().material.color = originalColors[0];
            originalColors.RemoveAt(0);
        }
    }
}
