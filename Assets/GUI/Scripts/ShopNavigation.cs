using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNavigation : MonoBehaviour
{

    public GameObject shopMenu;
    public void OpenShopMenu()
    {
        shopMenu.SetActive(true);
        Time.timeScale = 0f;
        MenuNavigation.IsPaused = true;
    }
    public void CloseShopMenu()
    {
        shopMenu.SetActive(false);
        Time.timeScale = 1f;
        MenuNavigation.IsPaused = false;
    }
}
