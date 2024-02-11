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
    public static RefinementMenuNavigation Instance;
    public GameObject refinementMenu;

    private int PutCheap = 0;
    private int PutExpensive = 0;
    private int TakeCheap = 0;
    private int TakeExpensive = 0;

    public void UpdateText()
    {
        RefinementMenuGUIValues.Instance.UpdateText(PutCheap, PutExpensive, TakeCheap, TakeExpensive);
    }

    public void CloseRefinementMenu()
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
        CloseRefinementMenu();
    }

    void Start()
    {
        if (Instance == null) Instance = this;
    }
}




