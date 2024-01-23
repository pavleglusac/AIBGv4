using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public GameObject PillarObject { get; set; }

    public GameObject housePrefab { get; set; }

    public PillarState PillarState { get; set; } = PillarState.Empty;
    public PillarState LastState { get; set; } = PillarState.Empty;

    public int X { get; set; }
    public int Z { get; set; }

    public List<Pillar> path;
    // keep track of original color
    List<Color> originalColors = new List<Color>();

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && PillarState == PillarState.Empty)
        {
            Player player = Game.Instance.GetCurrentPlayer();

            int price = int.Parse(PlayerPrefs.GetString("refinement_facility_cost"));
            if (!CanAct(player) || player.Coins < price)
            {
                // TODO: koristi Penalty funkciju umesto TakeEnergy
                int penalty = int.Parse(PlayerPrefs.GetString("invalid_turn_energy_penalty"));
                player.TakeEnergy(penalty);
                Game.Instance.SwitchPlayersAndDecreaseStats();
                return;
            }

            MakeHouse(player);
            player.TakeCoins(price);
            Debug.Log(player.Coins);
            Game.Instance.SwitchPlayersAndDecreaseStats();

        }
    }



    void OnMouseDown()
    {
        Move();

    }

    void OnMouseEnter()
    {
        Player player = Game.Instance.GetCurrentPlayer();

        if (!(CanStep() || CanAct(player)))
        {
            return;
        }

        if (Game.IsPaused)
            return;

        Pillar to = this;
        Pillar from;
        Color color;

        if (Game.Instance.FirstPlayerTurn)
        {
            from = Game.Instance.Player1.Position;
            color = Color.blue;
        }
        else
        {
            from = Game.Instance.Player2.Position;
            color = Color.red;
        }

        if (to.X != from.X && to.Z != from.Z)
        {
            return;
        }

        path = Algorithms.findPath(Game.Instance.Board, from, to);
        foreach (Pillar pillar in path)
        {
            originalColors.Add(pillar.PillarObject.GetComponent<Renderer>().material.color);
            pillar.PillarObject.GetComponent<Renderer>().material.color = color;
        }
    }

    public void Move()
    {
        int penalty = int.Parse(PlayerPrefs.GetString("invalid_turn_energy_penalty"));
        if (Game.IsPaused)
            return;

        Player player = Game.Instance.GetCurrentPlayer();

        if (path == null || path.Count == 0)
        {
            player.TakeEnergy(penalty);
            Game.Instance.SwitchPlayersAndDecreaseStats();
            return;
        }

        if (CanStep())
        {
            Actions.Move(this, player);
        }
        else if (CanAct(player))
        {

            // TODO add logic for non-empty pillars
            if (PillarState == PillarState.CheapCrystal || PillarState == PillarState.ExpensiveCrystal)
            {
                Actions.Mine(this, player);
            }

        }
        else
        {
            player.TakeEnergy(penalty);
            Game.Instance.SwitchPlayersAndDecreaseStats();
        }
        Game.Instance.TurnCount++;

    }

    void OnMouseExit()
    {
        if (Game.IsPaused)
            return;
        if (path == null || path.Count == 0 || originalColors.Count == 0)
        {
            return;
        }

        foreach (Pillar pillar in path)
        {
            pillar.PillarObject.GetComponent<Renderer>().material.color = originalColors[0];
            originalColors.RemoveAt(0);
        }
    }

    public bool CanStep()
    {
        if (PillarState == PillarState.Empty)
        {
            return true;
        }
        else if (Game.Instance.FirstPlayerTurn && PillarState == PillarState.BasePlayer1)
        {
            return true;
        }
        else if (!Game.Instance.FirstPlayerTurn && PillarState == PillarState.BasePlayer2)
        {
            return true;
        }
        return false;
    }


    bool CanAct(Player player)
    {
        List<Pillar> neighbours = Game.Instance.Board.getNeighbours(this);
        if (neighbours.Contains(player.Position))
        {
            return true;
        }
        return false;
    }

    public override bool Equals(object other)
    {
        Pillar pillar = (other as Pillar);
        return pillar.X == X && pillar.Z == Z && pillar.PillarState == PillarState;
    }

    private void MakeHouse(Player player)
    {

        Game.Instance.Board.Pillars[X, Z].PillarState = PillarState.House;
        // TODO code bellow breaks (housePrefab is null)
        Vector3 pillarPosition = PillarObject.transform.position;
        float x = pillarPosition.x;
        float y = 0.65f;
        float z = pillarPosition.z;
        GameObject houseObject = Instantiate(housePrefab, new Vector3(x, y, z), Quaternion.identity);
        if (!player.FirstPlayer)
        {
            Debug.Log("FIRST PLAYER MJAU");
            Material redMat = Resources.Load("RedHouseMaterial", typeof(Material)) as Material;
            houseObject.GetComponent<Renderer>().material = redMat;
        }

        houseObject.AddComponent<House>();
        houseObject.GetComponent<House>().HouseParentObject = houseObject;
        houseObject.GetComponent<House>().Position = this;
        houseObject.GetComponent<House>().X = this.X;
        houseObject.GetComponent<House>().Z = this.Z;

        houseObject.AddComponent<ExpensiveCrystal>();
        houseObject.GetComponent<ExpensiveCrystal>().Crystal2Object = houseObject;
        houseObject.GetComponent<ExpensiveCrystal>().SetPosition(Game.Instance.Board.Pillars[X, Z]);
        Game.Instance.Board.Houses.Add(houseObject.GetComponent<House>());

        // Animator animator = houseObject.GetComponent<Animator>();

        // if (animator != null)
        // {
        //    animator.enabled = false;
        // }
    }


}
