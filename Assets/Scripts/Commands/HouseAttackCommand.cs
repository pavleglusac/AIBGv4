using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseAttackCommand : MonoBehaviour, ICommand
{
    public Player Player { get; set; }
    public House House { get; set; }
    public bool isDone { get; set; } = false;
    public int X, Z;

    public HouseAttackCommand Initialize(Player player, int x, int z)
    {
        Player = player;
        X = x;
        Z = z;
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
            Player.AddCoins(int.Parse(PlayerPrefs.GetString("house_destroy_reward_coins")));
            Player.AddXP(int.Parse(PlayerPrefs.GetString("house_destroy_reward_xp")));
            Game.Instance.DisplayMessage = "Refinement facility successfully destroyed";
        }
        else
        {
            Game.Instance.DisplayMessage = "Refinement facility attacked!";
        }

        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }
    public bool CanExecute()
    {
        House = Game.Instance.Board.FindHouse(X, Z);

        if (House == null)
        {
            Game.Instance.DisplayMessage = "Not a refinement facility!";
            return false;
        }
        

        if (House.IsFirstPlayers == Player.FirstPlayer)
        {
            Game.Instance.DisplayMessage = "Cannot attack your own refinement facility";
            return false;
        }

        if (!House.Position.CanAct(Player))
        {
            Game.Instance.DisplayMessage = "You need to be near the opponent's refinement facility to attack it";
            return false;
        }

        Player oppositePlayer = Game.Instance.GetAlternatePlayer();
        var oppX = oppositePlayer.X;
        var oppZ = oppositePlayer.Z;

        var diffX = Mathf.Abs(oppX - X);
        var diffZ = Mathf.Abs(oppZ - Z);
        if (diffX <= 1 && diffZ <= 1)
        {
            Game.Instance.DisplayMessage = "Opponent is next to the refinement facility!";
            return false;
        }


        return true;
    }
}
