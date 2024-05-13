using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        transform.Rotate(Vector3.up, Time.deltaTime * 50f);

    }

    void OnMouseOver()
    {
        // Added so that players can not click 3D objects trough UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!Game.Instance.ArePlayersLanded) return;
        // log to console
        if (Input.GetMouseButtonDown(1))
        {
            if (Game.Instance.GetCurrentPlayer().X != this.X || Game.Instance.GetCurrentPlayer().Z != this.Z) return;
            BaseActionsMenu.Instance.OpenActionsMenu();
        }
    }

    void OnMouseDown()
    {
        // Added so that players can not click 3D objects trough UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!Game.Instance.ArePlayersLanded) return;

        Actions.Move(this.X, this.Z);
    }
}
