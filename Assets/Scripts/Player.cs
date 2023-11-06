using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject PlayerParentObject { get; set; }
    public GameObject PlayerObject { get; set; }
    public bool FirstPlayer { get; set; }
    public int X { get; set; }
    public int Z { get; set; }

    // TODO Jovan: Added now for hud logic, change later 
    public int XP { get; set; }
    public int Coins { get; set; }
    public int Energy { get; set; }
    public string Name { get; set; }




    void Start()
    {

    }


    void Update()
    {

    }


    void OnMouseDown()
    {
        // Debug.Log("Player clicked");
    }
}
