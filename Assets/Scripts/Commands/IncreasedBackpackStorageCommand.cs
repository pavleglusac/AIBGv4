using UnityEngine;

public class IncreasedBackpackStorageCommand : MonoBehaviour, ICoinSpendingCommand
{
    public Player Player { get; set; }
    public bool isDone { get; set; } = false;

    public IncreasedBackpackStorageCommand Initialize(Player player)
    {
        this.Player = player;
        return this;
    }

    public void Execute()
    {
        Player.AddIncreasedBackpackStorageTurns();
        Player.TakeCoins(int.Parse(PlayerPrefs.GetString("bigger_backpack_cost")));
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
        return Player.Coins >= int.Parse(PlayerPrefs.GetString("bigger_backpack_cost"));
    }
}