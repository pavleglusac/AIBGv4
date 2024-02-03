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
            Player player = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1 : Game.Instance.Player2;

            if (!CanAct(player))
            {
                int penalty = int.Parse(PlayerPrefs.GetString("invalid_turn_energy_penalty"));
                player.TakeEnergy(penalty);
                Game.Instance.FirstPlayerTurn = !Game.Instance.FirstPlayerTurn;
                return;
            }
<<<<<<< Updated upstream
            MakeHouse();
            int price = int.Parse(PlayerPrefs.GetString("refinement_facility_cost"));
=======


            MakeHouse(player);
>>>>>>> Stashed changes
            player.TakeCoins(price);
            Debug.Log(player.Coins);
            Game.Instance.FirstPlayerTurn = !Game.Instance.FirstPlayerTurn;

        }
    }



    void OnMouseDown()
    {
        Move();

    }

    void OnMouseEnter()
    {
        Player player = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1 : Game.Instance.Player2;

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

        Player player = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1 : Game.Instance.Player2;

        if (path == null || path.Count == 0)
        {
            player.TakeEnergy(penalty);
            Game.Instance.FirstPlayerTurn = !Game.Instance.FirstPlayerTurn;
            return;
        }

        if (CanStep())
        {
            Actions.Move(this, player);
        }
        else if (CanAct(player))
        {

            // TODO add logic for non-empty pillars
            if(PillarState == PillarState.CheapCrystal || PillarState == PillarState.ExpensiveCrystal)
            {
                Actions.Mine(this, player);
            }
            
        }
        else {
            player.TakeEnergy(penalty);
            Game.Instance.FirstPlayerTurn = !Game.Instance.FirstPlayerTurn;
        }
        Game.Instance.TurnCount++;
        Game.Instance.UpdateAllPlayerStats();
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

    private void MakeHouse() {

        Game.Instance.Board.Pillars[X, Z].PillarState = PillarState.House;
        // TODO code bellow breaks (housePrefab is null)
        //GameObject houseObject = Instantiate(housePrefab, new Vector3(X * Game.Instance.spacing, -50, Z * Game.Instance.spacing), Quaternion.identity, this.transform);
        //houseObject.AddComponent<ExpensiveCrystal>();
        //houseObject.GetComponent<ExpensiveCrystal>().Crystal2Object = houseObject;
        //houseObject.GetComponent<ExpensiveCrystal>().SetPosition(Game.Instance.Board.Pillars[X, Z]);
        //Game.Instance.Board.Houses.Add(houseObject.GetComponent<House>());

        //Animator animator = houseObject.GetComponent<Animator>();

        //if (animator != null)
        //{
        //    animator.enabled = false;
        //}
    }
        

}
