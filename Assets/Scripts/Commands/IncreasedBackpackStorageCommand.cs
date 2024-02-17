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
        Player.TakeCoins(GetCoinCost());
        Game.Instance.DisplayMessage = "Backpack storage increase successful!";

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
            Game.Instance.DisplayMessage = "Not enough coins for backpack capacity increase";
            return false;
        }

        return true;
    }

    public int GetCoinCost()
    {
        return int.Parse(PlayerPrefs.GetString("bigger_backpack_cost"));
    }
}