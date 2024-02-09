using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// inherit from command
public class BuildHouseCommand : MonoBehaviour, ICoinSpendingCommand
{
    public int Direction { get; set; }
    public Player Player { get; set; }
    public Pillar Pillar { get; set; }
    public bool isDone { get; set; } = false;

    public BuildHouseCommand Initialize(Player player, Pillar pillar)
    {
        Pillar = pillar;
        Player = player;
        return this;
    }

    public void Execute()
    {
        Game.Instance.Board.Pillars[Pillar.X, Pillar.Z].PillarState = PillarState.House;
        Vector3 pillarPosition = Pillar.PillarObject.transform.position;
        float x = pillarPosition.x;
        float y = 0.65f;
        float z = pillarPosition.z;
        GameObject houseObject = Instantiate(Pillar.housePrefab, new Vector3(x, y, z), Quaternion.identity);
        if (!Player.FirstPlayer)
        {
            Material redMat = Resources.Load("RedHouseMaterial", typeof(Material)) as Material;
            houseObject.GetComponent<Renderer>().material = redMat;
        }

        houseObject.AddComponent<House>();
        houseObject.GetComponent<House>().HouseParentObject = houseObject;
        houseObject.GetComponent<House>().Position = Pillar;
        houseObject.GetComponent<House>().X = Pillar.X;
        houseObject.GetComponent<House>().Z = Pillar.Z;

        houseObject.AddComponent<ExpensiveCrystal>();
        houseObject.GetComponent<ExpensiveCrystal>().Crystal2Object = houseObject;
        houseObject.GetComponent<ExpensiveCrystal>().SetPosition(Game.Instance.Board.Pillars[Pillar.X, Pillar.Z]);
        Game.Instance.Board.Houses.Add(houseObject.GetComponent<House>());

        Player.TakeCoins(GetCoinCost());

        Game.Instance.SwitchPlayersAndDecreaseStats();
        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }

    public bool CanExecute()
    {
        return Pillar.CanAct(Player) && Player.Coins >= GetCoinCost();
    }

    public int GetCoinCost()
    {
        return int.Parse(PlayerPrefs.GetString("refinement_facility_cost"));
    }
}
