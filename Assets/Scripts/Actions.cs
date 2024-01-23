using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
    public static void Mine(Pillar pillar, Player player)
    {
        GameObject commandObject = new GameObject("MineCommandObject");
        MineCommand mineCommandInstance = commandObject.AddComponent<MineCommand>();
        mineCommandInstance.Initialize(player, pillar.PillarState == PillarState.CheapCrystal);
        Game.Instance.CommandManager.AddCommand(mineCommandInstance);

        // swap player turn
        Game.Instance.SwitchPlayersAndDecreaseStats();

        if (pillar.PillarState == PillarState.CheapCrystal)
        {
            player.TakeEnergy(int.Parse(PlayerPrefs.GetString("mining_energy_cheap_crystal_loss")));
        }
        else
        {
            player.TakeEnergy(int.Parse(PlayerPrefs.GetString("mining_energy_expensive_crystal_loss")));
        }
    }

    public static void Move(Pillar pillar, Player player)
    {
        int penalty = int.Parse(PlayerPrefs.GetString("invalid_turn_energy_penalty"));
        if (!pillar.path.Contains(pillar))
        {
            player.TakeEnergy(penalty);
            return;
        }

        //Pillar prev = path[0];
        Pillar prev = player.Position;
        Pillar next = pillar;
        int count = pillar.path.Count;
        int direction = GetDirection(prev, next);

        // create commands
        GameObject commandObject = new GameObject("MoveCommandObject");
        MoveCommand moveCommandInstance = commandObject.AddComponent<MoveCommand>();
        moveCommandInstance.Initialize(player, direction, count);
        Game.Instance.CommandManager.AddCommand(moveCommandInstance);

        // swap player turn
        player.Position.PillarState = player.Position.LastState;
        pillar.LastState = pillar.PillarState;
        pillar.PillarState = Game.Instance.FirstPlayerTurn ? PillarState.Player1 : PillarState.Player2;
        Game.Instance.SwitchPlayersAndDecreaseStats();
        player.SetPosition(pillar);
        player.TakeEnergy(count * (player.Bag.GetWeight() + 1));
    }

    private static int GetDirection(Pillar prev, Pillar current)
    {
        if (prev.X == current.X)
        {
            return prev.Z > current.Z ? 0 : 2;
        }
        else if (prev.Z == current.Z)
        {
            return prev.X > current.X ? 1 : 3;
        }
        return -1;
    }
}