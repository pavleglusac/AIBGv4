using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActionsMenu : MonoBehaviour
{

    private int XPCheap = 0;
    private int XPExpensive = 0;
    private int CoinsCheap = 0;
    private int CoinsExpensive = 0;
    private int EnergyCheap = 0;
    private int EnergyExpensive = 0;


    public static BaseActionsMenu Instance;
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

    void Start()
    {
        // Debug.Log("Usao");
        // CloseActionsMenu();
        if (Instance == null) Instance = this;
    }

    public void IncreaseXPCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < 5)
        {
            Instance.XPCheap++;
        }
        Debug.Log(Instance.XPCheap);
        // UpdateText();
    }

    public void DecreaseXPCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        if (Instance.XPCheap > 0)
        {
            Instance.XPCheap--;
        }
        Debug.Log(Instance.XPCheap);
        // UpdateText();
    }

    public void IncreaseXPExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < player.Bag.GetCheapCrystalCount())
        if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < 5)
        {
            Instance.XPExpensive++;
        }
        Debug.Log(Instance.XPExpensive);
        // UpdateText();
    }

    public void DecreaseXPExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        if (Instance.XPExpensive > 0)
        {
            Instance.XPExpensive--;
        }
        Debug.Log(Instance.XPExpensive);
        // UpdateText();
    }


    public void IncreaseCoinsCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < 5)
        {
            Instance.CoinsCheap++;
        }
        Debug.Log(Instance.CoinsCheap);
        // UpdateText();
    }

    public void DecreaseCoinsCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        if (Instance.CoinsCheap > 0)
        {
            Instance.CoinsCheap--;
        }
        Debug.Log(Instance.CoinsCheap);
        // UpdateText();
    }

    public void IncreaseCoinsExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < player.Bag.GetCheapCrystalCount())
        if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < 5)
        {
            Instance.CoinsExpensive++;
        }
        Debug.Log(Instance.CoinsExpensive);
        // UpdateText();
    }

    public void DecreaseCoinsExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        if (Instance.CoinsExpensive > 0)
        {
            Instance.XPExpensive--;
        }
        Debug.Log(Instance.XPExpensive);
        // UpdateText();
    }


    public void IncreaseEnergyCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < 5)
        {
            Instance.EnergyCheap++;
        }
        Debug.Log(Instance.EnergyCheap);
        // UpdateText();
    }

    public void DecreaseEnergyCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        if (Instance.EnergyCheap > 0)
        {
            Instance.EnergyCheap--;
        }
        Debug.Log(Instance.EnergyCheap);
        // UpdateText();
    }

    public void IncreaseEnergyExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < player.Bag.GetCheapCrystalCount())
        if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < 5)
        {
            Instance.EnergyExpensive++;
        }
        Debug.Log(Instance.EnergyExpensive);
        // UpdateText();
    }

    public void DecreaseEnergyExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        if (Instance.EnergyExpensive > 0)
        {
            Instance.EnergyExpensive--;
        }
        Debug.Log(Instance.EnergyExpensive);
        // UpdateText();
    }
}
