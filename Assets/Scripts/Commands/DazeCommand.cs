using UnityEngine;

public class DazeCommand : MonoBehaviour, ICoinSpendingCommand
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
        Player.TakeCoins(GetCoinCost());
        Game.Instance.DisplayMessage = "Daze Successful!";
        isDone = true;
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
        return Player.Coins >= GetCoinCost();
    }

    public int GetCoinCost()
    {
        return int.Parse(PlayerPrefs.GetString("daze_cost"));
    }
}