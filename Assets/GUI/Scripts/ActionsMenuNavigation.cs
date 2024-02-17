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
        Actions.Freeze();

        CloseActionsMenu();
    }


    public void BuyDaze()
    {

        Actions.Daze();
        CloseActionsMenu();
    }


    public void BuyIncreasedBackpackStorage()
    {
        Actions.IncreaseBacpackStorage();
        CloseActionsMenu();
    }

    public void ActivateRest()
    {
        Actions.Rest();
        CloseActionsMenu();
    }



}
