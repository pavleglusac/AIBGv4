using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Actions
{

    public static void Move(int x, int z)
    {
        GameObject commandObject = new GameObject("MoveCommandObject");
        MoveCommand moveCommandInstance = commandObject.AddComponent<MoveCommand>();
        moveCommandInstance.Initialize(x, z);
        Game.Instance.CommandManager.AddCommand(moveCommandInstance);
    }

    public static void Mine(int x, int z)
    {
        GameObject commandObject = new GameObject("MineCommandObject");
        MineCommand mineCommandInstance = commandObject.AddComponent<MineCommand>();
        mineCommandInstance.Initialize(Game.Instance.GetCurrentPlayer(), x, z);
        Game.Instance.CommandManager.AddCommand(mineCommandInstance);
    }

    public static void BuildHouse(int x, int z)
    {
        if ((x == Game.Instance.BasePlayer1.X && z == Game.Instance.BasePlayer1.Z) || (x == Game.Instance.BasePlayer2.X && z == Game.Instance.BasePlayer2.Z)) return;
        GameObject commandObject = new GameObject("BuildHouseObject");
        BuildHouseCommand buildHouseCommandInstance = commandObject.AddComponent<BuildHouseCommand>();
        buildHouseCommandInstance.Initialize(x, z);
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
}