using UnityEngine;

public class RestCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }
    public bool isDone { get; set; } = false;

    public RestCommand Initialize(Player player)
    {
        this.Player = player;
        return this;
    }

    public void Execute()
    {

        //Player.IncreaseEnergy(int.Parse(PlayerPrefs.GetString("resting_energy")));
        Debug.Log(Player.Energy);
        Player.IncreaseEnergy(10);
        Debug.Log(Player.Energy);
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