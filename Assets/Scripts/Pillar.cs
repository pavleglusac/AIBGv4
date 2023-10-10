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
            Pillar pillar = path[i];
            // print prev and pillar
            Debug.Log(prev.X + " " + prev.Z + " ---- " + pillar.X + " " + pillar.Z);
            Debug.Log(direction + " " + count);
            if (prev.X == pillar.X)
            {
                if (prev.Z > pillar.Z)
                {
                    if (direction == 0)
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
                        direction = 0;
                    }
                }
                else if (prev.Z < pillar.Z)
                {
                    if (direction == 2)
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
                        direction = 2;
                    }
                }
            }
            else if (prev.Z == pillar.Z)
            {
                if (prev.X > pillar.X)
                {
                    if (direction == 1)
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
                        direction = 1;
                    }
                }
                else if (prev.X < pillar.X)
                {
                    if (direction == 3)
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
                        direction = 3;
                    }
                }
            }
            prev = pillar;
        }

        directions.Add((direction, count));

        // attach movementmanager to player if it doesn't exist
        Player player = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1 : Game.Instance.Player2;
        if (player.PlayerObject.GetComponent<MovementManager>() == null)
        {
            player.PlayerObject.AddComponent<MovementManager>();
            player.PlayerObject.GetComponent<MovementManager>().player = player.PlayerObject;
        }
        for (int i = 0; i < directions.Count; i++)
        {
            (int, int) dir = directions[i];
            player.PlayerObject.GetComponent<MovementManager>().AddMovement(dir.Item1, dir.Item2);
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
            color = Color.green;
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
