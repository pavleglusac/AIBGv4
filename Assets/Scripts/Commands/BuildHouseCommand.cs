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

    private int sentX;
    private int sentZ;

    public BuildHouseCommand Initialize(int x, int z)
    {
        Player = Game.Instance.GetCurrentPlayer();
        sentX = x;
        sentZ = z;
        
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
        houseObject.GetComponent<House>().Health = int.Parse(PlayerPrefs.GetString("house_health"));
        houseObject.GetComponent<House>().IsFirstPlayers = Player.FirstPlayer;
        //set first child of house to be invisible
        houseObject.transform.GetChild(0).gameObject.SetActive(false);


        houseObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0f, 0f, 0.5f));

        if (!Player.FirstPlayer)
        {
            houseObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0f, 0f));
        }

        Game.Instance.Board.Houses.Add(houseObject.GetComponent<House>());


        Player.TakeCoins(GetCoinCost());

        Game.Instance.DisplayMessage = "Refinement facility succesfuly built!";
        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }

    public bool CanExecute()
    {
        if (sentX < 0 || sentX >= Game.Instance.Board.Width || sentZ < 0 || sentZ >= Game.Instance.Board.Height)
        {
            Game.Instance.DisplayMessage = "Build out of bounds";
            return false;
        }

        Pillar = Game.Instance.Board.Pillars[sentX, sentZ];
        
        if (Pillar.PillarState != PillarState.Empty)
        {
            Game.Instance.DisplayMessage = "Pillar is not empty";
            return false;
        }
        if (!Pillar.CanAct(Player))
        {
            Game.Instance.DisplayMessage = "You are not close enough to build";
            return false;
        }
        if (Player.Coins < GetCoinCost())
        {
            Game.Instance.DisplayMessage = "Not enough coins";
            return false;
        }


        return true;
    }

    public int GetCoinCost()
    {
        return int.Parse(PlayerPrefs.GetString("refinement_facility_cost"));
    }
}
