using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft;


[DefaultExecutionOrder(2)]
public class ResourceGenerator : MonoBehaviour
{
    public Game game;
    public GameObject crystal1Prefab;
    public GameObject crystal2Prefab;
    public Transform cameraTransform;
    [HideInInspector] public int rows;
    [HideInInspector] public int columns;
    [HideInInspector] public float spacing;
    [HideInInspector] public float animationDelay;
    [HideInInspector] public int numberOfCheapCrystalGroups;
    [HideInInspector] public int numberOfExpensiveCrystalGroups;
    [HideInInspector] public int numberOfCheapCrystalsInGroup;
    [HideInInspector] public int numberOfExpensiveCrystalsInGroup;
    public int baseAreaLength;
    public static System.Random random = new System.Random();

    // Start :called before the first frame update
    void Start()
    {
        // clear log file
        game = Game.Instance;
        rows = game.rows;
        columns = game.columns;
        spacing = game.spacing;
        animationDelay = game.animationDelay;
        numberOfCheapCrystalGroups = game.numberOfCheapCrystalGroups;
        numberOfExpensiveCrystalGroups = game.numberOfExpensiveCrystalGroups;
        numberOfCheapCrystalsInGroup = game.numberOfCheapCrystalsInGroup;
        numberOfExpensiveCrystalsInGroup = game.numberOfExpensiveCrystalsInGroup;
        baseAreaLength = rows / 3;

        //Debug.Log("numberOfCheapCrystalGroups: " + numberOfCheapCrystalGroups + " numberOfExpensiveCrystalGroups: " + numberOfExpensiveCrystalGroups + " numberOfCheapCrystalsInGroup: " + numberOfCheapCrystalsInGroup + " numberOfExpensiveCrystalsInGroup: " + numberOfExpensiveCrystalsInGroup);

        //game.Board.CheapCrystals = new CheapCrystal[(numberOfCheapCrystalGroups * numberOfCheapCrystalsInGroup * 2)]; // 3 crystals per group, 2 groups one for each side
        //game.Board.ExpensiveCrystals = new ExpensiveCrystal[(numberOfExpensiveCrystalGroups * numberOfExpensiveCrystalsInGroup * 2)]; // 3 crystals per group, 2 groups one for each side

        //GenerateCrystals(false);
        //GenerateCrystals(true);

        //StartCoroutine(StartAnimations());
        int index = random.Next(LevelData.Levels.Count);
        LevelData.ItemData level = LevelData.Levels[index];
        var count = 0;

        var cheapCount = 0;
        foreach (var crystal in level.cheap)
        {
            if (crystal[0] != crystal[1])
            {
                cheapCount++;
            }
            cheapCount++;
        }

        var expensiveCount = 0;
        foreach (var crystal in level.expensive)
        {
            if (crystal[0] != crystal[1])
            {
                expensiveCount++;
            }
            expensiveCount++;
        }


        game.Board.CheapCrystals = new CheapCrystal[cheapCount]; // 3 crystals per group, 2 groups one for each side
        game.Board.ExpensiveCrystals = new ExpensiveCrystal[expensiveCount]; // 3 crystals per group, 2 groups one for each side

        foreach (var crystal in level.cheap)
        {
            MakeCrystalCheap(crystal[0], crystal[1], count);
            count++;
            if (crystal[0] == crystal[1])
            {
                continue;
            }
            MakeCrystalCheap(crystal[1], crystal[0], count);
            count++;
        }
        count = 0;
        foreach (var crystal in level.expensive)
        {
            MakeCrystalExpensive(crystal[0], crystal[1], count);
            count++;
            if (crystal[0] == crystal[1])
            {
                continue;
            }
            MakeCrystalExpensive(crystal[1], crystal[0], count);
            count++;
        }
        StartCoroutine(StartAnimations());
    }


    public List<Tuple<int, int>> FindFreeCells(bool up)
    {
        List<Tuple<int, int>> freeCells = new List<Tuple<int, int>>();

        for (int i = 0; i < game.Board.Pillars.GetLength(0); i++)
        {
            for (int j = 0; j < game.Board.Pillars.GetLength(1); j++)
            {
                int x = game.Board.Pillars[i, j].X;
                int z = game.Board.Pillars[i, j].Z;

                if (x <= z) continue;

                bool inSquare = x > rows - baseAreaLength - 1 && z < baseAreaLength;
                bool isUp = (12 - x) > z;
                if (up && !(isUp && !(inSquare))) continue;

                if (!up && !(!isUp && !(inSquare))) continue;


                if (game.Board.Pillars[i, j].PillarState != PillarState.Empty) continue;
                freeCells.Add(new Tuple<int, int>(x, z));
            }
        }

        return freeCells;
    }


    public Tuple<int, int> GenerateCoordinates(bool up)
    {

        List<Tuple<int, int>> freeCells = FindFreeCells(up);
        freeCells = freeCells.OrderBy(_ => random.Next()).ToList();

        foreach (Tuple<int, int> cell in freeCells)
        {
            return cell;
        }
        return new Tuple<int, int>(-1, -1);

    }

    bool CheckIfCoordinatesAreValid(int x, int z)
    {
        if (rows - baseAreaLength <= x && x <= rows - 1 && 0 <= z && z <= baseAreaLength - 1)
        {
            return false;
        }
        if (x < 0 || x >= rows || z < 0 || z >= columns || x == z)
        {
            return false;
        }
        return true;
    }

    bool CheckCenterCoordinates(int x, int z, int groupSize)
    {
        if (x < 0 || z < 0) return true;
        if (game.Board.Pillars[x, z].PillarState != PillarState.Empty)
        {
            return true;
        }
        int count = 1;
        foreach ((int dx, int dz) in new[] { (0, 1), (0, -1), (-1, 0), (1, 0) })
        {
            int newX = x + dx;
            int newZ = z + dz;

            if (!CheckIfCoordinatesAreValid(newX, newZ))
            {
                continue;
            }

            if (game.Board.Pillars[x, z].PillarState == PillarState.Empty)
            {
                count++;
            }
        }
        return count < groupSize;
    }


    void GenerateCrystals(bool isExpensive)
    {

        int generatedCrystals = 0;

        int numOfGroups = isExpensive ? numberOfExpensiveCrystalGroups : numberOfCheapCrystalGroups;
        int numberOfCrystalsInGroup = isExpensive ? numberOfExpensiveCrystalsInGroup : numberOfCheapCrystalsInGroup;
        Tuple<int, int>[] groupCoordinates = new Tuple<int, int>[numOfGroups];
        List<Tuple<int, int>> crystalsCoordinates = new List<Tuple<int, int>>();

        bool up = true;
        for (int i = 0; i < groupCoordinates.Length; i++)
        {
            bool canNotBeCenter = true;
            Tuple<int, int> coordinates = new(0, 0);
            int tryNumber = 0;
            bool canNotFind = false;
            while (canNotBeCenter)
            {
                tryNumber++;
                coordinates = GenerateCoordinates(up);
                if (coordinates.Item1 == -1)
                {
                    numberOfCrystalsInGroup--;
                    if (numberOfCrystalsInGroup == 0)
                    {
                        canNotFind = true;
                        break;
                    }
                    continue;
                }
                canNotBeCenter = CheckCenterCoordinates(coordinates.Item1, coordinates.Item2, numberOfCrystalsInGroup);
            }

            if (canNotFind)
                break;

            int x = coordinates.Item1;
            int z = coordinates.Item2;
            up = !up;

            groupCoordinates[i] = new Tuple<int, int>(x, z);
            for (int j = 0; j < i; j++)
            {
                if ((groupCoordinates[j].Item1 == x && groupCoordinates[j].Item2 == z) || (groupCoordinates[j].Item1 == z && groupCoordinates[j].Item2 == x))
                {
                    i--;
                    break;
                }
            }
            var groupCrystalsCoordinates = GenerateCrystalGroup(x, z, crystalsCoordinates, isExpensive, numberOfCrystalsInGroup);
            crystalsCoordinates.AddRange(groupCrystalsCoordinates);

            foreach (var t in groupCrystalsCoordinates)
            {
                int crystal_x = t.Item1;
                int crystal_z = t.Item2;
                if (isExpensive)
                {
                    MakeCrystalExpensive(crystal_x, crystal_z, generatedCrystals);
                }
                else
                {
                    MakeCrystalCheap(crystal_x, crystal_z, generatedCrystals);
                }
                generatedCrystals++;

                if (isExpensive)
                {
                    MakeCrystalExpensive(crystal_z, crystal_x, generatedCrystals);
                }
                else
                {
                    MakeCrystalCheap(crystal_z, crystal_x, generatedCrystals);
                }
                generatedCrystals++;
            }
        }
    }


    List<Tuple<int, int>> GenerateCrystalGroup(int x, int z, List<Tuple<int, int>> existingCrystals, bool isExpensive, int numberOfCrystalsInGroup)
    {
        List<Tuple<int, int>> newCrystalsCoordinates = new();
        List<int> directions = new() { 0, 1, 2, 3 };
        for (int i = 0; i < numberOfCrystalsInGroup; i++)
        {
            int xCoordinate = x;
            int zCoordinate = z;

            if (i != 0)
            {
                int direction = directions[random.Next(directions.Count)];
                directions.Remove(direction);
                if (directions.Count == 0)
                {
                    return newCrystalsCoordinates;
                }
                switch (direction)
                {
                    case 0:
                        xCoordinate++;
                        break;
                    case 1:
                        xCoordinate--;
                        break;
                    case 2:
                        zCoordinate++;
                        break;
                    case 3:
                        zCoordinate--;
                        break;
                }
            }

            if (!CheckIfCoordinatesAreValid(xCoordinate, zCoordinate) || existingCrystals.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)) || newCrystalsCoordinates.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)))
            {
                i--;
                continue;
            }

            if (game.Board.Pillars[xCoordinate, zCoordinate].PillarState == PillarState.Empty)
            {
                newCrystalsCoordinates.Add(new Tuple<int, int>(xCoordinate, zCoordinate));
                game.Board.Pillars[xCoordinate, zCoordinate].PillarState = isExpensive ? PillarState.ExpensiveCrystal : PillarState.CheapCrystal;
            }
            else
            {
                i--;
            }
        }
        return newCrystalsCoordinates;
    }




    void MakeCrystalExpensive(int x, int z, int crystal2Count)
    {
        game.Board.Pillars[x, z].PillarState = PillarState.ExpensiveCrystal;
        GameObject crystal2Object = Instantiate(crystal2Prefab, new Vector3(x * spacing, -50, z * spacing), Quaternion.identity, this.transform);
        crystal2Object.AddComponent<ExpensiveCrystal>();
        crystal2Object.GetComponent<ExpensiveCrystal>().Crystal2Object = crystal2Object;
        crystal2Object.GetComponent<ExpensiveCrystal>().SetPosition(game.Board.Pillars[x, z]);
        game.Board.ExpensiveCrystals[crystal2Count] = crystal2Object.GetComponent<ExpensiveCrystal>();


        Animator animator = crystal2Object.GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = false;
        }

    }


    void MakeCrystalCheap(int x, int z, int crystal1Count)
    {
        game.Board.Pillars[x, z].PillarState = PillarState.CheapCrystal;
        GameObject crystal1Object = Instantiate(crystal1Prefab, new Vector3(x * spacing, -50, z * spacing), Quaternion.identity, this.transform);
        crystal1Object.AddComponent<CheapCrystal>();
        crystal1Object.GetComponent<CheapCrystal>().Crystal1Object = crystal1Object;
        game.Board.CheapCrystals[crystal1Count] = crystal1Object.GetComponent<CheapCrystal>();
        game.Board.CheapCrystals[crystal1Count].SetPosition(game.Board.Pillars[x, z]);

        Animator animator = crystal1Object.GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    IEnumerator StartAnimations()
    {
        yield return new WaitForSeconds(animationDelay);

        for (int i = 0; i < game.Board.CheapCrystals.Length; i++)
        {
            if (game.Board.CheapCrystals[i] == null) continue;
            Animator animator = game.Board.CheapCrystals[i].Crystal1Object.GetComponent<Animator>();
            if (animator == null) continue;
            animator.enabled = true;
            animator.speed = 1.0f;
            animator.SetTrigger("Crystal1GrowingTrigger");
        }

        for (int i = 0; i < game.Board.ExpensiveCrystals.Length; i++)
        {
            if (game.Board.ExpensiveCrystals[i] == null) continue;
            Animator animator = game.Board.ExpensiveCrystals[i].Crystal2Object.GetComponent<Animator>();
            if (animator == null) continue;
            animator.enabled = true;
            animator.speed = 1.0f;
            animator.SetTrigger("Crystal2GrowingTrigger");
        }

        // activate trigger on player 1
        // player1.GetComponent<Animator>().SetTrigger("UFO2LandingTrigger");
        // player2.GetComponent<Animator>().SetTrigger("UFO1LandingTrigger");

        // // activate trigger on base 1
        // basePlayer1.GetComponent<Animator>().SetTrigger("Base1BuildingTrigger");
        // basePlayer2.GetComponent<Animator>().SetTrigger("Base2BuildingTrigger");  
    }


}
