using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class RefinementMenuGUIValues : MonoBehaviour
{

    public static RefinementMenuGUIValues Instance;
    public Text BagCheap;
    public Text BagExpensive;

    public Text PutCheapText;
    public Text PutExpensiveText;

    public Text ProcessingCheap;
    public Text ProcessingExpensive;

    public Text DoneCheap;
    public Text DoneExpensive;

    public Text TakeCheapText;
    public Text TakeExpensiveText;


    //update text
    public void UpdateText(int PutCheap, int PutExpensive, int TakeCheap, int TakeExpensive)
    {
        Player player = Game.Instance.GetCurrentPlayer();
        BagCheap.text = "Minerals: " + player.Bag.GetCheapCrystalCount().ToString();
        BagExpensive.text = "Diamonds: " + player.Bag.GetExpensiveCrystalCount().ToString();
        ProcessingCheap.text = "Minerals: " + Game.Instance.selectedHouse.GetUnprocessedCheapCrystalCount().ToString();
        ProcessingExpensive.text = "Diamonds: " + Game.Instance.selectedHouse.GetUnprocessedExpensiveCrystalCount().ToString();
        DoneCheap.text = "Minerals: " + Game.Instance.selectedHouse.GetProcessedCheapCrystalCount().ToString();
        DoneExpensive.text = "Diamonds: " + Game.Instance.selectedHouse.GetProcessedExpensiveCrystalCount().ToString();
        PutCheapText.text = PutCheap.ToString();
        PutExpensiveText.text = PutExpensive.ToString();
        TakeCheapText.text = TakeCheap.ToString();
        TakeExpensiveText.text = TakeExpensive.ToString();

    }


    void Start()
    {

        if (Instance == null) Instance = this;
    }
}




