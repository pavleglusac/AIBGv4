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
        AlternatePlayer.AddFrozenTurns();
        Player.TakeCoins(GetCoinCost());
        Game.Instance.DisplayMessage = "Freeze Successful!";
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
        if (Player.Coins < GetCoinCost())
        {
            Game.Instance.DisplayMessage = "Not enough coins for freeze";
            return false;
        }

        return true;
    }

    public int GetCoinCost()
    {
        return int.Parse(PlayerPrefs.GetString("freeze_cost"));
    }
}