using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class RefinementMenuNavigation : MonoBehaviour
{
    public GameObject refinementMenu;
    public static RefinementMenuNavigation Instance;
    private int PutCheap = 0;
    private int PutExpensive = 0;
    private int TakeCheap = 0;
    private int TakeExpensive = 0;

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
    public void UpdateText()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        BagCheap.text = "Cheap: " + player.Bag.GetCheapCrystalCount().ToString();
        BagExpensive.text = "Expensive: " + player.Bag.GetExpensiveCrystalCount().ToString();
        ProcessingCheap.text = "Cheap: " + Game.Instance.selectedHouse.GetUnprocessedCheapCrystalCount().ToString();
        ProcessingExpensive.text = "Expensive: " + Game.Instance.selectedHouse.GetUnprocessedExpensiveCrystalCount().ToString();
        DoneCheap.text = "Cheap: " + Game.Instance.selectedHouse.GetProcessedCheapCrystalCount().ToString();
        DoneExpensive.text = "Expensive: " + Game.Instance.selectedHouse.GetProcessedExpensiveCrystalCount().ToString();
        PutCheapText.text = PutCheap.ToString();
        PutExpensiveText.text = PutExpensive.ToString();
        TakeCheapText.text = TakeCheap.ToString();
        TakeExpensiveText.text = TakeExpensive.ToString();

    }
    

    public void CloseActionsMenu()
    {
        refinementMenu.SetActive(false);
        Game.Instance.selectedHouse = null;
        Game.ResumeGame();
    }

    public void OpenRefinementMenu()
    {
        if (Game.IsPaused)
            return;
        refinementMenu.SetActive(true);
        UpdateText();
        Game.PauseGame();
    }

    public void IncreasePutCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (PutCheap < player.Bag.GetCheapCrystalCount())
        {
            PutCheap++;
        }
        UpdateText();
    }

    public void DecreasePutCheapCrystal()
    {
        if (PutCheap > 0)
        {
            PutCheap--;
        }
        UpdateText();
    }

    public void IncreasePutExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (PutExpensive < player.Bag.GetExpensiveCrystalCount())
        {
            PutExpensive++;
        }
        UpdateText();
    }

    public void DecreasePutExpensiveCrystal()
    {
        if (PutExpensive > 0)
        {
            PutExpensive--;
        }
        UpdateText();
    }

    public void Put()
    {
        Actions.PutRefinement(Game.Instance.GetCurrentPlayer(), Game.Instance.selectedHouse, PutCheap, PutExpensive);
    }

   



    void Update()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("RefinementMenuNavigation started");
        if (Instance == null) Instance = this;
    }
}




