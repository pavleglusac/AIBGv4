using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            player.DecreaseEnergy(int.Parse(PlayerPrefs.GetString("mining_energy_cheap_crystal_loss")));
        }
        else
        {
            player.DecreaseEnergy(int.Parse(PlayerPrefs.GetString("mining_energy_expensive_crystal_loss")));
        }
    }

    public static void Move(Pillar enteredPillar, Player player)
    {
        Debug.Log("eNTERED: " + enteredPillar.X + " " + enteredPillar.Z);
        Pillar pillar;
        if (Game.Instance.GetCurrentPlayer().IsDazed())
        {
            if (OutOfBounds(enteredPillar, player))
            {
                player.InvalidMoveTakeEnergy();
                Game.Instance.SwitchPlayersAndDecreaseStats();
                return;
            }

            pillar = ChangePillarsBasedOnDaze(enteredPillar, player);
        }
        else
        {
            pillar = enteredPillar;
        }


        var path = Algorithms.findPath(Game.Instance.Board, player.Position, pillar);
        if (!path.Contains(pillar))
        {
            player.InvalidMoveTakeEnergy();
            Game.Instance.SwitchPlayersAndDecreaseStats();
            return;
        }

        //Pillar prev = path[0];
        Pillar prev = player.Position;
        Pillar next = pillar;
        int count = Math.Abs(pillar.X - player.X) + Math.Abs(pillar.Z - player.Z);
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

        player.SetPosition(pillar);
        player.DecreaseEnergy(count * (player.Bag.GetWeight() + 1));
        Game.Instance.SwitchPlayersAndDecreaseStats();
    }

    private static bool OutOfBounds(Pillar pillar, Player player)
    {
        int stepX = Math.Abs(pillar.X - player.X);
        int stepZ = Math.Abs(pillar.Z - player.Z);

        int newX = player.X - stepX;
        int newZ = player.Z - stepZ;

        newX = (pillar.X > player.X) ? newX : player.X + stepX;
        newZ = (pillar.Z > player.Z) ? newZ : player.Z + stepZ;
        int boardSize = int.Parse(PlayerPrefs.GetString("board_size")) - 1;

        return newX < 0 || newX > boardSize || newZ < 0 || newZ > boardSize;
    }

    private static Pillar ChangePillarsBasedOnDaze(Pillar pillar, Player player)
    {
        int stepX = Math.Abs(pillar.X - player.X);
        int stepZ = Math.Abs(pillar.Z - player.Z);

        int newX = player.X - stepX;
        int newZ = player.Z - stepZ;

        newX = (pillar.X > player.X) ? newX : player.X + stepX;
        newZ = (pillar.Z > player.Z) ? newZ : player.Z + stepZ;

        return Game.Instance.Board.Pillars[newX, newZ];
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