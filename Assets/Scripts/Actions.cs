using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Actions
{

    public static void Mine(int x, int z)
    {
        GameObject commandObject = new GameObject("MineCommandObject");
        MineCommand mineCommandInstance = commandObject.AddComponent<MineCommand>();
        mineCommandInstance.Initialize(Game.Instance.GetCurrentPlayer(), x, z);
        Game.Instance.CommandManager.AddCommand(mineCommandInstance);
    }

    public static void BuildHouse(Player player, Pillar pillar)
    {
        GameObject commandObject = new GameObject("BuildHouseObject");
        BuildHouseCommand buildHouseCommandInstance = commandObject.AddComponent<BuildHouseCommand>();
        buildHouseCommandInstance.Initialize(player, pillar);
        Game.Instance.CommandManager.AddCommand(buildHouseCommandInstance);
    }

    public static void PutRefinement(Player player, House house, int putCheap, int putExpensive)
    {
        GameObject commandObject = new GameObject("RefinementPutCommandObject");
        RefinementPutCommand refinementPutCommandInstance = commandObject.AddComponent<RefinementPutCommand>();
        refinementPutCommandInstance.Initialize(player, putCheap, putExpensive, house);
        Game.Instance.CommandManager.AddCommand(refinementPutCommandInstance);
    }

    public static void TakeRefinement(Player player, House house, int takeCheap, int takeExpensive)
    {
        GameObject commandObject = new GameObject("RefinementTakeCommandObject");
        RefinementTakeCommand refinementTakeCommandInstance = commandObject.AddComponent<RefinementTakeCommand>();
        refinementTakeCommandInstance.Initialize(player, takeCheap, takeExpensive, house);
        Game.Instance.CommandManager.AddCommand(refinementTakeCommandInstance);
    }

    public static void BaseConversions(Player player, int cheapTotal, int expensiveTotal, int XPTotal, int coinsTotal, int energyTotal)
    {
        GameObject commandObject = new GameObject("ConversionsCommandObject");
        ConversionCommand conversionsCommand = commandObject.AddComponent<ConversionCommand>();
        conversionsCommand.Initialize(player, cheapTotal, expensiveTotal, XPTotal, coinsTotal, energyTotal);
        Game.Instance.CommandManager.AddCommand(conversionsCommand);
    }




    public static void Daze()
    {
        GameObject commandObject = new GameObject("DazeCommand");
        DazeCommand commandInstance = commandObject.AddComponent<DazeCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer(), Game.Instance.GetAlternatePlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
    }
    public static void Freeze()
    {

        GameObject commandObject = new GameObject("FreezeCommand");
        FreezeCommand mineCommandInstance = commandObject.AddComponent<FreezeCommand>();
        mineCommandInstance.Initialize(Game.Instance.GetCurrentPlayer(), Game.Instance.GetAlternatePlayer());
        Game.Instance.CommandManager.AddCommand(mineCommandInstance);
    }
    public static void IncreaseBacpackStorage()
    {

        GameObject commandObject = new GameObject("IncreasedBackpackStorageCommand");
        IncreasedBackpackStorageCommand commandInstance = commandObject.AddComponent<IncreasedBackpackStorageCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
    }


    public static void Rest()
    {
        GameObject commandObject = new GameObject("RestCommand");
        RestCommand commandInstance = commandObject.AddComponent<RestCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
    }

    public static void Move(Pillar enteredPillar, Player player)
    {
        Pillar pillar;
        if (Game.Instance.GetCurrentPlayer().IsDazed())
        {
            if (OutOfBounds(enteredPillar, player))
            {
                player.InvalidMoveTakeEnergy();
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
        // player.DecreaseEnergy(count * (player.Bag.GetWeight() + 1));
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