using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal1 : MonoBehaviour
{

    public GameObject Crystal1ParentObject {get; set;}
    public GameObject Crystal1Object {get; set; }
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
        if (!CanAnimate())
        {
            return;
        }
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
            animator.speed = 4.0f;
            animator.SetTrigger("ShakeCrystal1Trigger");
        }
        Position.Move();
    }


    bool CanAnimate()
    {
        List<Pillar> neighbours = Game.Instance.Board.getNeighbours(Position);
        if (neighbours.Contains(Game.Instance.GetCurrentPlayer().Position))
        {
            return true;
        }
        return false;
    }

    public void SetPosition(Pillar pillar)
    {
        Position = pillar;
        X = Position.X;
        Z = Position.Z;
    }

    
}
