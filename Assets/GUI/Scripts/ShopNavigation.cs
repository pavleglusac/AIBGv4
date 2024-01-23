using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNavigation : MonoBehaviour
{

    public GameObject shopMenu;
    public void OpenShopMenu()
    {
        if (Game.IsPaused)
            return;
        shopMenu.SetActive(true);
        Game.PauseGame();
    }
    public void CloseShopMenu()
    {
        shopMenu.SetActive(false);
        Game.ResumeGame();
    }


    public void BuyFreeze()
    {

        if (Game.Instance.GetCurrentPlayer().Coins < int.Parse(PlayerPrefs.GetString("freeze_cost")))
        {
            Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
            Game.Instance.SwitchPlayersAndDecreaseStats();
            CloseShopMenu();
            return;
        }
        GameObject commandObject = new GameObject("FreezeCommand");
        FreezeCommand mineCommandInstance = commandObject.AddComponent<FreezeCommand>();
        mineCommandInstance.Initialize(Game.Instance.GetCurrentPlayer(), Game.Instance.GetAlternatePlayer());
        Game.Instance.CommandManager.AddCommand(mineCommandInstance);

        CloseShopMenu();
    }


    public void BuyDaze()
    {
        if (Game.Instance.GetCurrentPlayer().Coins < int.Parse(PlayerPrefs.GetString("daze_cost")))
        {
            Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
            Game.Instance.SwitchPlayersAndDecreaseStats();
            CloseShopMenu();
            return;
        }
        Game.Instance.GetAlternatePlayer().AddDazeTurns();
        Game.Instance.GetCurrentPlayer().TakeCoins(int.Parse(PlayerPrefs.GetString("daze_cost")));
        Game.Instance.SwitchPlayersAndDecreaseStats();
        CloseShopMenu();
    }


    public void BuyIncreasedBackpackStorage()
    {
        if (Game.Instance.GetCurrentPlayer().Coins < int.Parse(PlayerPrefs.GetString("bigger_backpack_cost")))
        {
            Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
            Game.Instance.SwitchPlayersAndDecreaseStats();
            CloseShopMenu();
            return;
        }
        Game.Instance.GetCurrentPlayer().AddIncreasedBackpackStorageTurns();
        Game.Instance.GetCurrentPlayer().TakeCoins(int.Parse(PlayerPrefs.GetString("bigger_backpack_cost")));
        Game.Instance.SwitchPlayersAndDecreaseStats();
        CloseShopMenu();
    }


}
