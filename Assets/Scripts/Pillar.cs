using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        if (MenuNavigation.IsPaused)
            return;

        if (path == null || path.Count == 0)
        {
            return;
        }

        Pillar prev = path[0];
        Pillar next = this;
        int count = path.Count;
        int direction = GetDirection(prev, next);

        Player player = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1 : Game.Instance.Player2;
        // create commands
        GameObject commandObject = new GameObject("MoveCommandObject");
        MoveCommand moveCommandInstance = commandObject.AddComponent<MoveCommand>();
        moveCommandInstance.Initialize(player, direction, count);
        Game.Instance.CommandManager.AddCommand(moveCommandInstance);

        // swap player turn
        Game.Instance.FirstPlayerTurn = !Game.Instance.FirstPlayerTurn;
        player.SetPosition(this);
    }

    void OnMouseEnter()
    {
        if (MenuNavigation.IsPaused)
            return;

        Pillar to = this;
        Pillar from;
        Color color;

        if (Game.Instance.FirstPlayerTurn)
        {
            from = Game.Instance.Player1.Position;
            color = Color.blue;
        }
        else
        {
            from = Game.Instance.Player2.Position;
            color = Color.red;
        }

        if (to.X != from.X && to.Z != from.Z) {
            return;
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
        if (MenuNavigation.IsPaused)
            return;
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
