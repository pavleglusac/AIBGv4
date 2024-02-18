using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    public GameObject BaseParentObject { get; set; }
    public GameObject BaseObject { get; set; }
    public bool IsFirstPlayers { get; set; }
    public int X { get; set; }
    public int Z { get; set; }
    // Start is called before the first frame update
    void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log(this.X + " " + this.Z);
            Debug.Log("Name: " + gameObject.name);
            if (this.X == 0 && this.Z == 0) return;
            Debug.Log(this.IsFirstPlayers);
            if (Game.Instance.GetCurrentPlayer().X != this.X || Game.Instance.GetCurrentPlayer().Z != this.Z) return;
            //             if (Game.Instance.GetCurrentPlayer().FirstPlayer)
            // {
            //     if (Game.Instance.GetCurrentPlayer().X != Game.Instance.Board.Bases[0].X || Game.Instance.GetCurrentPlayer().Z != Game.Instance.Board.Bases[0].Z) return;
            // }
            // else
            // {
            //     if (Game.Instance.GetCurrentPlayer().X != Game.Instance.Board.Bases[1].X || Game.Instance.GetCurrentPlayer().Z != Game.Instance.Board.Bases[1].Z) return;
            // }
            BaseActionsMenu.Instance.OpenActionsMenu();
        }

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Actions.BuildHouse(this.X, this.Z);
        }
        if (Input.GetKey(KeyCode.Mouse2))
        {
            // Debug.Log(this.X + " " + this.Z);
            // Debug.Log("Name: " + gameObject.name);
            if (this.X == 0 && this.Z == 0) return;
            // Debug.Log(this.IsFirstPlayers);
            if (Game.Instance.GetCurrentPlayer().X != this.X || Game.Instance.GetCurrentPlayer().Z != this.Z) return;
            // if (Game.Instance.GetCurrentPlayer().FirstPlayer != this.IsFirstPlayers) return;
            BaseActionsMenu.Instance.OpenActionsMenu();
        }
    }
}
