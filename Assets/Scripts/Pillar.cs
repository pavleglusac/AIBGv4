using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pillar : MonoBehaviour
{

    public GameObject PillarObject { get; set; }

    public PillarState PillarState { get; set; } = PillarState.Empty;
    public PillarState LastState { get; set; } = PillarState.Empty;

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
        Move();
        
    }

    void OnMouseEnter()
    {
        Player player = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1 : Game.Instance.Player2;

        if (!(CanStep() || CanAct(player))) {
            return;
        }

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

    public void Move()
    {
        if (MenuNavigation.IsPaused)
            return;

        if (path == null)
        {
            return;
        }

        Player player = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1 : Game.Instance.Player2;

        if (CanStep())
        {

            //Pillar prev = path[0];
            Pillar prev = player.Position;
            Pillar next = this;
            int count = path.Count;
            int direction = GetDirection(prev, next);

            // create commands
            GameObject commandObject = new GameObject("MoveCommandObject");
            MoveCommand moveCommandInstance = commandObject.AddComponent<MoveCommand>();
            moveCommandInstance.Initialize(player, direction, count);
            Game.Instance.CommandManager.AddCommand(moveCommandInstance);

            // swap player turn
            player.Position.PillarState = player.Position.LastState;
            LastState = PillarState;
            PillarState = Game.Instance.FirstPlayerTurn ? PillarState.Player1 : PillarState.Player2;
            Game.Instance.FirstPlayerTurn = !Game.Instance.FirstPlayerTurn;
            player.SetPosition(this);
            player.TakeEnergy(count);
        }
        else if (CanAct(player))
        {
            // TODO add logic for non-empty pillars
            Debug.Log("ACT");
        }
    }

    void OnMouseExit()
    {
        if (MenuNavigation.IsPaused)
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
        if (PillarState == PillarState.Empty) {
            return true;
        }
        else if (Game.Instance.FirstPlayerTurn && PillarState == PillarState.BasePlayer1) {
            return true;
        }
        else if (!Game.Instance.FirstPlayerTurn && PillarState == PillarState.BasePlayer2)
        {
            return true;
        }
        return false;
    }
    

    bool CanAct(Player player) {
        List<Pillar> neighbours = Game.Instance.Board.getNeighbours(this);
        if (neighbours.Contains(player.Position)) {
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
