using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public GameObject PillarObject { get; set; }

    public GameObject housePrefab { get; set; }

    public PillarState PillarState { get; set; } = PillarState.Empty;
    public PillarState LastState { get; set; } = PillarState.Empty;

    public int X { get; set; }
    public int Z { get; set; }

    public List<Pillar> path;
    // keep track of original color
    List<Color> originalColors = new List<Color>();

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Actions.BuildHouse(this.X, this.Z);
        }
    }

    void OnMouseDown()
    {
        Actions.Move(this.X, this.Z);
    }

    void OnMouseEnter()
    {
        Player player = Game.Instance.GetCurrentPlayer();

        if (!(CanStep() || CanAct(player)))
        {
            return;
        }

        if (Game.IsPaused)
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

        if (to.X != from.X && to.Z != from.Z)
        {
            return;
        }

        path = Algorithms.FindPath(Game.Instance.Board, from, to);
        foreach (Pillar pillar in path)
        {
            originalColors.Add(pillar.PillarObject.GetComponent<Renderer>().material.color);
            pillar.PillarObject.GetComponent<Renderer>().material.color = color;
        }
    }

    void OnMouseExit()
    {
        if (Game.IsPaused)
            return;
        if (path == null || path.Count == 0 || originalColors.Count == 0)
        {
            return;
        }

        foreach (Pillar pillar in path)
        {
            pillar.PillarObject.GetComponent<Renderer>().material.color = originalColors[0];
            originalColors.RemoveAt(0);
        }
    }

    public bool CanStep()
    {
        if (PillarState == PillarState.Empty)
        {
            return true;
        }
        else if (Game.Instance.FirstPlayerTurn && PillarState == PillarState.BasePlayer1)
        {
            return true;
        }
        else if (!Game.Instance.FirstPlayerTurn && PillarState == PillarState.BasePlayer2)
        {
            return true;
        }
        return false;
    }

    public bool CanAct(Player player)
    {
        List<Pillar> neighbours = Game.Instance.Board.getNeighbours(this);
        if (neighbours.Contains(player.Position))
        {
            return true;
        }
        return false;
    }

    public override bool Equals(object other)
    {
        Pillar pillar = (other as Pillar);
        return pillar.X == X && pillar.Z == Z && pillar.PillarState == PillarState;
    }
}
