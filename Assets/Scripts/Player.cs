using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject PlayerParentObject {get; set;}
    public GameObject PlayerObject {get; set;}
    public bool FirstPlayer {get; set;}
    public int X {get; set;}
    public int Z {get; set;}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when the player is clicked
    void OnMouseDown()
    {
        // Debug.Log("Player clicked");
    }
}
