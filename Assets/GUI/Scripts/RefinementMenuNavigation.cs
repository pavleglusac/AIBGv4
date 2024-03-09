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

    private House selectedHouse;

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
        Instance.selectedHouse = null;
        Game.ResumeGame();
        Instance.PutCheap = 0;
        Instance.PutExpensive = 0;
        Instance.TakeCheap = 0;
        Instance.TakeExpensive = 0;
    }

    public void OpenRefinementMenu(House house)
    {
        if (Game.IsPaused)
            return;
        selectedHouse = house;
        Instance.selectedHouse = house;
        Game.Instance.selectedHouse = house;
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
        Actions.PutRefinement(Instance.selectedHouse.X, Instance.selectedHouse.Z, Instance.PutCheap, Instance.PutExpensive);
        CloseRefinementMenu();
    }

    public void IncreaseTakeCheapCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        Debug.Log(Instance.selectedHouse);
        if (Instance.TakeCheap == Instance.selectedHouse.GetProcessedCheapCrystalCount()) return;
        int processedCheapCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_cheap_crystal_weight"));
        int processedExpensiveCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_expensive_crystal_weight"));
        if (player.Bag.GetRemainingCapacity() < (Instance.TakeCheap + 1) * processedCheapCrystalWeight + Instance.TakeExpensive * processedExpensiveCrystalWeight) return;
        Instance.TakeCheap++;
        UpdateText();
    }

    public void DecreaseTakeCheapCrystal()
    {
        if (Instance.TakeCheap > 0)
        {
            Instance.TakeCheap--;
        }
        UpdateText();
    }

    public void IncreaseTakeExpensiveCrystal()
    {
        Player player = Game.Instance.GetCurrentPlayer();
        if (Instance.TakeExpensive == Instance.selectedHouse.GetProcessedExpensiveCrystalCount()) return;
        int processedCheapCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_cheap_crystal_weight"));
        int processedExpensiveCrystalWeight = int.Parse(PlayerPrefs.GetString("processed_expensive_crystal_weight"));
        if (player.Bag.GetRemainingCapacity() < (Instance.TakeExpensive + 1) * processedExpensiveCrystalWeight + Instance.TakeCheap * processedCheapCrystalWeight) return;
        Instance.TakeExpensive++;
        UpdateText();
    }

    public void DecreaseTakeExpensiveCrystal()
    {
        if (Instance.TakeExpensive > 0)
        {
            Instance.TakeExpensive--;
        }
        UpdateText();
    }

    public void Take()
    {
        Actions.TakeRefinement(Instance.selectedHouse.X, Instance.selectedHouse.Z, Instance.TakeCheap, Instance.TakeExpensive);
        CloseRefinementMenu();
    }

    void Start()
    {
        // Debug.Log("Usao");
        if (Instance == null) Instance = this;
    }
}




