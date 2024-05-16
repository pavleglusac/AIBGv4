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
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        MoveCommand moveCommandInstance = commandObject.AddComponent<MoveCommand>();
        moveCommandInstance.Initialize(x, z);
        Game.Instance.CommandManager.AddCommand(moveCommandInstance);
    }

    public static void Mine(int x, int z)
    {
        GameObject commandObject = new GameObject("MineCommandObject");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        MineCommand mineCommandInstance = commandObject.AddComponent<MineCommand>();
        mineCommandInstance.Initialize(Game.Instance.GetCurrentPlayer(), x, z);
        Game.Instance.CommandManager.AddCommand(mineCommandInstance);
    }

    public static void BuildHouse(int x, int z)
    {
        GameObject commandObject = new GameObject("BuildHouseObject");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        BuildHouseCommand buildHouseCommandInstance = commandObject.AddComponent<BuildHouseCommand>();
        buildHouseCommandInstance.Initialize(x, z);
        Game.Instance.CommandManager.AddCommand(buildHouseCommandInstance);
    }

    public static void AttackHouse(int x, int z)
    {
        Player player = Game.Instance.GetCurrentPlayer();
        GameObject commandObject = new GameObject("AttackHouseObject");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        HouseAttackCommand attackHouseCommandInstance = commandObject.AddComponent<HouseAttackCommand>();
        attackHouseCommandInstance.Initialize(player, x, z);
        Game.Instance.CommandManager.AddCommand(attackHouseCommandInstance);
    }

    public static void PutRefinement(int x, int z, int putCheap, int putExpensive)
    {
        Player player = Game.Instance.GetCurrentPlayer();
        GameObject commandObject = new GameObject("RefinementPutCommandObject");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        RefinementPutCommand refinementPutCommandInstance = commandObject.AddComponent<RefinementPutCommand>();
        refinementPutCommandInstance.Initialize(player, putCheap, putExpensive, x, z);
        Game.Instance.CommandManager.AddCommand(refinementPutCommandInstance);
    }

    public static void TakeRefinement(int x, int z, int takeCheap, int takeExpensive)
    {
        GameObject commandObject = new GameObject("RefinementTakeCommandObject");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        Player player = Game.Instance.GetCurrentPlayer();
        RefinementTakeCommand refinementTakeCommandInstance = commandObject.AddComponent<RefinementTakeCommand>();
        refinementTakeCommandInstance.Initialize(player, takeCheap, takeExpensive, x, z);
        Game.Instance.CommandManager.AddCommand(refinementTakeCommandInstance);
    }

    public static void BaseConversions(int XPCheap, int XPExpensive, int coinsCheap, int coinsExpensive, int energyCheap, int energyExpensive)
    {
        Player player = Game.Instance.GetCurrentPlayer();
        GameObject commandObject = new GameObject("ConversionsCommandObject");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        ConversionCommand conversionsCommand = commandObject.AddComponent<ConversionCommand>();
        conversionsCommand.Initialize(player, XPCheap, XPExpensive, coinsCheap, coinsExpensive, energyCheap, energyExpensive);
        Game.Instance.CommandManager.AddCommand(conversionsCommand);
    }


    public static void Daze()
    {
        GameObject commandObject = new GameObject("DazeCommand");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        DazeCommand commandInstance = commandObject.AddComponent<DazeCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer(), Game.Instance.GetAlternatePlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
    }

    public static void Freeze()
    {
        GameObject commandObject = new GameObject("FreezeCommand");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        FreezeCommand mineCommandInstance = commandObject.AddComponent<FreezeCommand>();
        mineCommandInstance.Initialize(Game.Instance.GetCurrentPlayer(), Game.Instance.GetAlternatePlayer());
        Game.Instance.CommandManager.AddCommand(mineCommandInstance);
    }

    public static void IncreaseBacpackStorage()
    {
        GameObject commandObject = new GameObject("IncreasedBackpackStorageCommand");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        IncreasedBackpackStorageCommand commandInstance = commandObject.AddComponent<IncreasedBackpackStorageCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
    }


    public static void Rest()
    {
        GameObject commandObject = new GameObject("RestCommand");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        RestCommand commandInstance = commandObject.AddComponent<RestCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
    }

    public static void InvalidAction()
    {
        GameObject commandObject = new GameObject("InvalidActionCommand");
        commandObject.transform.SetParent(GameObject.Find("Commands").transform ?? null, false);
        InvalidActionCommand commandInstance = commandObject.AddComponent<InvalidActionCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
    }
}