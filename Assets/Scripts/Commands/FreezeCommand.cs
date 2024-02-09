using UnityEngine;

public class FreezeCommand : MonoBehaviour, ICoinSpendingCommand
{
    public Player Player { get; set; }
    public Player AlternatePlayer { get; set; }
    public bool isDone { get; set; } = false;

    public FreezeCommand Initialize(Player player, Player alternatePlayer)
    {
        this.Player = player;
        this.AlternatePlayer = alternatePlayer;
        return this;
    }

    public void Execute()
    {
        Game.Instance.GetAlternatePlayer().AddFrozenTurns();
        Game.Instance.GetCurrentPlayer().TakeCoins(int.Parse(PlayerPrefs.GetString("freeze_cost")));
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

    public bool CanExecute()
    {
        return Game.Instance.GetCurrentPlayer().Coins >= int.Parse(PlayerPrefs.GetString("freeze_cost"));
    }
}