using UnityEngine;

public class InvalidActionCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }
    public bool isDone { get; set; } = false;

    public InvalidActionCommand Initialize(Player player)
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

    public bool CanExecute()
    {
        return false;
    }

}