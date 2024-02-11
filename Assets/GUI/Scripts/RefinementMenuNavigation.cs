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
        RefinementMenuGUIValues.Instance.UpdateText(Instance.PutCheap, Instance.PutExpensive, Instance.TakeCheap, Instance.TakeExpensive);
    }

    public void CloseRefinementMenu()
    {
        refinementMenu.SetActive(false);
        Game.Instance.selectedHouse = null;
        Game.ResumeGame();
        Instance.PutCheap = 0;
        Instance.PutExpensive = 0;
        Instance.TakeCheap = 0;
        Instance.TakeExpensive = 0;
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
        if (Instance.PutCheap < player.Bag.GetCheapCrystalCount())
        {
            Instance.PutCheap++;
        }
        UpdateText();
    }

    public void DecreasePutCheapCrystal()
    {
        if (Instance.PutCheap > 0)
        {
            Instance.PutCheap--;
        }
        UpdateText();
    }

    public void IncreasePutExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (Instance.PutExpensive < player.Bag.GetExpensiveCrystalCount())
        {
            Instance.PutExpensive++;
        }
        UpdateText();
    }

    public void DecreasePutExpensiveCrystal()
    {
        if (Instance.PutExpensive > 0)
        {
            Instance.PutExpensive--;
        }
        UpdateText();
    }

    public void Put()
    {
        Actions.PutRefinement(Game.Instance.GetCurrentPlayer(), Game.Instance.selectedHouse, Instance.PutCheap, Instance.PutExpensive);
        CloseRefinementMenu();
    }

    void Start()
    {
        if (Instance == null) Instance = this;
    }
}




