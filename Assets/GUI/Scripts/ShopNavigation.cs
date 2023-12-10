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
}
