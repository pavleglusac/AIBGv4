using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    public GameObject TreeParentObject {get; set;}
    public GameObject TreeObject {get; set; }
    public Pillar Position { get; set; }
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
        Debug.Log("tree");
        Position.Move();
    }


    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }
}
