using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseAttackCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }
    public House House { get; set; }
    private bool isDone = false;

    public HouseAttackCommand Initialize(Player player, House house)
    {
        Player = player;
        House = house;
        return this;
    }

    public void Execute()
    {
        int damage = int.Parse(PlayerPrefs.GetString("house_damage"));
        House.Health -= damage;
        Player.DecreaseEnergy(int.Parse(PlayerPrefs.GetString("house_attack_energy")));
        if (House.Health <= 0)
        {
            House.Destroy();
            Player.AddCoins(int.Parse(PlayerPrefs.GetString("house_destroy_reward")));
        }


        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }
    public bool CanExecute()
    {
        if (!House.Position.CanAct(Player))
        {
            Game.Instance.DisplayMessage = "You need to be near the opponent house to attack it";
            return false;
        }

        return true;
    }
}
