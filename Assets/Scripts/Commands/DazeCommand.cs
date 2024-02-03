using UnityEngine;

public class DazeCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }
    public Player AlternatePlayer { get; set; }
    public bool isDone { get; set; } = false;

    public DazeCommand Initialize(Player player, Player alternatePlayer)
    {
        this.Player = player;
        this.AlternatePlayer = alternatePlayer;
        return this;
    }

    public void Execute()
    {
        AlternatePlayer.AddDazeTurns();
        Player.TakeCoins(int.Parse(PlayerPrefs.GetString("daze_cost")));
        Game.Instance.SwitchPlayersAndDecreaseStats();
        isDone = true;
    }

    public void Update()
    {

    }

    public bool IsDone()
    {
        return isDone;
    }
}