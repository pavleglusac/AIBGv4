using System;
using UnityEngine;

public class ActionsMenuNavigation : MonoBehaviour
{

    public GameObject actionMenu;
    public void OpenActionsMenu()
    {
        if (Game.IsPaused)
            return;
        actionMenu.SetActive(true);
        Game.PauseGame();
    }
    public void CloseActionsMenu()
    {
        actionMenu.SetActive(false);
        Game.ResumeGame();
    }


    public void BuyFreeze()
    {

        GameObject commandObject = new GameObject("FreezeCommand");
        FreezeCommand mineCommandInstance = commandObject.AddComponent<FreezeCommand>();
        mineCommandInstance.Initialize(Game.Instance.GetCurrentPlayer(), Game.Instance.GetAlternatePlayer());
        Game.Instance.CommandManager.AddCommand(mineCommandInstance);

        CloseActionsMenu();
    }


    public void BuyDaze()
    {

        GameObject commandObject = new GameObject("DazeCommand");
        DazeCommand commandInstance = commandObject.AddComponent<DazeCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer(), Game.Instance.GetAlternatePlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
        CloseActionsMenu();
    }


    public void BuyIncreasedBackpackStorage()
    {

        GameObject commandObject = new GameObject("IncreasedBackpackStorageCommand");
        IncreasedBackpackStorageCommand commandInstance = commandObject.AddComponent<IncreasedBackpackStorageCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
        CloseActionsMenu();
    }

    public void ActivateRest()
    {
        GameObject commandObject = new GameObject("RestCommand");
        RestCommand commandInstance = commandObject.AddComponent<RestCommand>();
        commandInstance.Initialize(Game.Instance.GetCurrentPlayer());
        Game.Instance.CommandManager.AddCommand(commandInstance);
        CloseActionsMenu();
    }



}
