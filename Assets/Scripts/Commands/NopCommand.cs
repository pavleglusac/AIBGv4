using UnityEngine;

public class NopCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }
    public bool isDone { get; set; } = false;

    public NopCommand Initialize(Player player)
    {
        this.Player = player;
        return this;
    }

    public void Execute()
    {
        
    }

    public void Update()
    {
        
    }

    public bool IsDone()
    {
        return isDone;
    }
}