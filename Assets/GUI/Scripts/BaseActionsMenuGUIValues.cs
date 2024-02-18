using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BaseActionsMenuGUI : MonoBehaviour
{
    public static BaseActionsMenuGUI Instance;

    public Text XPBagCheap;
    public Text XPBagExpensive;
    public Text XPCheap;
    public Text XPExpensive;

    public Text CoinsBagCheap;
    public Text CoinsBagExpensive;
    public Text CoinsCheap;
    public Text CoinsExpensive;

    public Text EnergyBagCheap;
    public Text EnergyBagExpensive;
    public Text EnergyCheap;
    public Text EnergyExpensive;

    public Text XPTotal;
    public Text CoinsTotal;


    public void UpdateText(int XPCheap, int XPExpensive, int CoinsCheap, int CoinsExpensive, int EnergyCheap, int EnergyExpensive, int XPTotal, int CoinsTotal)
    {
        Player player = Game.Instance.GetCurrentPlayer();
        XPBagCheap.text = "Cheap: " + player.Bag.GetCheapCrystalCount().ToString();
        XPBagExpensive.text = "Expensive: " + player.Bag.GetExpensiveCrystalCount().ToString();
        CoinsBagCheap.text = "Cheap: " + player.Bag.GetCheapCrystalCount().ToString();
        CoinsBagExpensive.text = "Expensive: " + player.Bag.GetExpensiveCrystalCount().ToString();
        EnergyBagCheap.text = "Cheap: " + player.Bag.GetCheapCrystalCount().ToString();
        EnergyBagExpensive.text = "Expensive: " + player.Bag.GetExpensiveCrystalCount().ToString();
        this.XPCheap.text = XPCheap.ToString();
        this.XPExpensive.text = XPExpensive.ToString();
        this.CoinsCheap.text = CoinsCheap.ToString();
        this.CoinsExpensive.text = CoinsExpensive.ToString();
        this.EnergyCheap.text = EnergyCheap.ToString();
        this.EnergyExpensive.text = EnergyExpensive.ToString();
        this.XPTotal.text = "  " + XPTotal.ToString() + " XP";
        this.CoinsTotal.text = " " + CoinsTotal.ToString() + " Coins";
    }

    void Start()
    {

        if (Instance == null) Instance = this;
    }
}
