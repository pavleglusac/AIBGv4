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

    private int XPCheapConversionRate = 0;
    private int XPExpensiveConversionRate = 0;
    private int CoinsCheapConversionRate = 0;
    private int CoinsExpensiveConversionRate = 0;
    private int EnergyCheapConversionRate = 0;
    private int EnergyExpensiveConversionRate = 0;

    private int CoinsTotal = 0;
    private int XPTotal = 0;
    private int EnergyTotal = 0;

    public static BaseActionsMenu Instance;
    public GameObject actionMenu;

    public void OpenActionsMenu()
    {
        if (Game.IsPaused)
            return;
        actionMenu.SetActive(true);
        UpdateText();
        Game.PauseGame();
    }

    public void CloseActionsMenu()
    {
        actionMenu.SetActive(false);
        Game.ResumeGame();
    }

    public void UpdateText()
    {
        BaseActionsMenuGUI.Instance.UpdateText(Instance.XPCheap, Instance.XPExpensive, Instance.CoinsCheap, Instance.CoinsExpensive, Instance.EnergyCheap, Instance.EnergyExpensive, Instance.XPTotal, Instance.CoinsTotal, Instance.EnergyTotal);
    }

    void Start()
    {
        // CloseActionsMenu();
        if (Instance == null) Instance = this;
    }

    public void IncreaseXPCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        // // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < 5)
        {
            Instance.XPCheap++;
            CalculateTotal();
        }
        UpdateText();
    }

    public void DecreaseXPCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (Instance.XPCheap > 0)
        {
            Instance.XPCheap--;
            CalculateTotal();
        }
        UpdateText();
    }

    public void IncreaseXPExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < player.Bag.GetExpensiveCrystalCount())
        // if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < 5)
        {
            Instance.XPExpensive++;
            CalculateTotal();
        }
        UpdateText();
    }

    public void DecreaseXPExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (Instance.XPExpensive > 0)
        {
            Instance.XPExpensive--;
            CalculateTotal();
        }
        UpdateText();
    }


    public void IncreaseCoinsCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < 5)
        {
            Instance.CoinsCheap++;
            CalculateTotal();
        }
        UpdateText();
    }

    public void DecreaseCoinsCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (Instance.CoinsCheap > 0)
        {
            Instance.CoinsCheap--;
            CalculateTotal();
        }
        UpdateText();
    }

    public void IncreaseCoinsExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < player.Bag.GetExpensiveCrystalCount())
        // if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < 5)
        {
            Instance.CoinsExpensive++;
            CalculateTotal();
        }
        UpdateText();
    }

    public void DecreaseCoinsExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (Instance.CoinsExpensive > 0)
        {
            Instance.CoinsExpensive--;
            CalculateTotal();
        }
        UpdateText();
    }


    public void IncreaseEnergyCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < player.Bag.GetCheapCrystalCount())
        // if ((Instance.XPCheap + Instance.CoinsCheap + Instance.EnergyCheap) < 5)
        {
            Instance.EnergyCheap++;
            CalculateTotal();
        }
        UpdateText();
    }

    public void DecreaseEnergyCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (Instance.EnergyCheap > 0)
        {
            Instance.EnergyCheap--;
            CalculateTotal();
        }
        UpdateText();
    }

    public void IncreaseEnergyExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < player.Bag.GetExpensiveCrystalCount())
        // if ((Instance.XPExpensive + Instance.CoinsExpensive + Instance.EnergyExpensive) < 5)
        {
            Instance.EnergyExpensive++;
            CalculateTotal();
        }
        UpdateText();
    }

    public void DecreaseEnergyExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (Instance.EnergyExpensive > 0)
        {
            Instance.EnergyExpensive--;
            CalculateTotal();
        }
        UpdateText();
    }

    public void CalculateTotal()
    {
        if (Instance.XPCheapConversionRate == 0) Instance.XPCheapConversionRate = int.Parse(PlayerPrefs.GetString("cheap_to_xp"));
        if (Instance.XPExpensiveConversionRate == 0) Instance.XPExpensiveConversionRate = int.Parse(PlayerPrefs.GetString("exp_to_xp"));
        if (Instance.CoinsCheapConversionRate == 0) Instance.CoinsCheapConversionRate = int.Parse(PlayerPrefs.GetString("cheap_to_coins"));
        if (Instance.CoinsExpensiveConversionRate == 0) Instance.CoinsExpensiveConversionRate = int.Parse(PlayerPrefs.GetString("exp_to_coins"));
        if (Instance.EnergyCheapConversionRate == 0) Instance.EnergyCheapConversionRate = int.Parse(PlayerPrefs.GetString("cheap_to_energy"));
        if (Instance.EnergyExpensiveConversionRate == 0) Instance.EnergyExpensiveConversionRate = int.Parse(PlayerPrefs.GetString("exp_to_energy"));

        Instance.XPTotal = Instance.XPCheap * Instance.XPCheapConversionRate + Instance.XPExpensive * Instance.XPExpensiveConversionRate;
        Instance.CoinsTotal = Instance.CoinsCheap * Instance.CoinsCheapConversionRate + Instance.CoinsExpensive * Instance.CoinsExpensiveConversionRate;
        Instance.EnergyTotal = Instance.EnergyCheap * Instance.EnergyCheapConversionRate + Instance.EnergyExpensive * Instance.EnergyExpensiveConversionRate;
    }

    public void CancelActions()
    {
        Instance.XPCheap = 0;
        Instance.XPExpensive = 0;
        Instance.CoinsCheap = 0;
        Instance.CoinsExpensive = 0;
        Instance.EnergyCheap = 0;
        Instance.EnergyExpensive = 0;
        CalculateTotal();
        UpdateText();
        CloseActionsMenu();
    }

    public void Convert()
    {
        Actions.BaseConversions(Instance.XPCheap, Instance.XPExpensive, Instance.CoinsCheap, Instance.CoinsExpensive, Instance.EnergyCheap, Instance.EnergyExpensive);
        CancelActions();
    }
}
