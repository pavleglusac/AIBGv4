using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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

    // Start is called before the first frame update
    void Start()
    {
        using StreamWriter writer = new("log.txt", true);
        writer.WriteLine("POCETAK GENERISANJA RESURSA!");
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

        game.Board.CheapCrystals = new CheapCrystal[(numberOfCheapCrystalGroups * numberOfCheapCrystalsInGroup * 2)]; // 3 crystals per group, 2 groups one for each side
        game.Board.ExpensiveCrystals = new ExpensiveCrystal[(numberOfExpensiveCrystalGroups * numberOfExpensiveCrystalsInGroup * 2)]; // 3 crystals per group, 2 groups one for each side
        writer.WriteLine("KRAJ INICIJALIZACIJE!");

        GenerateCrystals(false, writer);
        writer.WriteLine("KRAJ GENERISANJA JEFTINIH RESURSA!");
        GenerateCrystals(true, writer);
        writer.WriteLine("KRAJ GENERISANJA SKUPLJIH RESURSA!");

        writer.WriteLine("KRAJ GENERISANJA RESURSA!");
        StartCoroutine(StartAnimations());
        writer.WriteLine("KRAJ ANIMACIJA!");


    }


    public Tuple<int, int> GenerateCoordinates(bool up)
    {
        int x, z;
        if (up)
        {
            do
            {
                x = random.Next(1, rows - baseAreaLength - 1);
                z = random.Next(1, baseAreaLength + 1);
            } while (!(x > z));
        }
        else
        {
            do
            {
                x = random.Next(rows - baseAreaLength, rows);
                z = random.Next(baseAreaLength + 1, columns - 1);
            } while (!(x > z));
        }
        return new Tuple<int, int>(x, z);

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


    void GenerateCrystals(bool isExpensive, StreamWriter writer = null)
    {
        writer.WriteLine("isExpensive: " + isExpensive.ToString());
        int generatedCrystals = 0;

        int numOfGroups = isExpensive ? numberOfExpensiveCrystalGroups : numberOfCheapCrystalGroups;
        int numberOfCrystalsInGroup = isExpensive ? numberOfExpensiveCrystalsInGroup : numberOfCheapCrystalsInGroup;
        Tuple<int, int>[] groupCoordinates = new Tuple<int, int>[numOfGroups];
        List<Tuple<int, int>> crystalsCoordinates = new List<Tuple<int, int>>();

        bool up = true;
        for (int i = 0; i < groupCoordinates.Length; i++)
        {
            numberOfCrystalsInGroup = isExpensive ? numberOfExpensiveCrystalsInGroup : numberOfCheapCrystalsInGroup;
            writer.WriteLine("group number: " + i.ToString() + " up: " + up.ToString() + " length: " + groupCoordinates.Length.ToString());
            bool canNotBeCenter = true;
            Tuple<int, int> coordinates = new(0, 0);
            int tryNumber = 0;
            bool canNotFind = false;
            while (canNotBeCenter)
            {
                tryNumber++;
                writer.WriteLine("tryNumber: " + tryNumber);
                if (tryNumber > 225)
                {
                    if (numberOfCrystalsInGroup > 0)
                    {
                        numberOfCrystalsInGroup--;
                        tryNumber = 0;
                    }
                    else
                    {
                        canNotFind = true;
                        break;
                    }
                }
                coordinates = GenerateCoordinates(up);
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
            writer.WriteLine("x: " + x.ToString() + " z: " + z.ToString());
            var groupCrystalsCoordinates = GenerateCrystalGroup(x, z, crystalsCoordinates, isExpensive, numberOfCrystalsInGroup);
            writer.WriteLine("groupCrystalsCoordinates: " + groupCrystalsCoordinates.Count.ToString());
            crystalsCoordinates.AddRange(groupCrystalsCoordinates);

            for (int j = 0; j < groupCrystalsCoordinates.Count; j++)
            {
                writer.WriteLine("j: " + j.ToString());
                int crystal_x = groupCrystalsCoordinates[j].Item1;
                int crystal_z = groupCrystalsCoordinates[j].Item2;
                if (isExpensive)
                {
                    MakeCrystal2(crystal_x, crystal_z, generatedCrystals);
                }
                else
                {
                    MakeCrystal1(crystal_x, crystal_z, generatedCrystals);
                }
                generatedCrystals++;

                if (isExpensive)
                {
                    MakeCrystal2(crystal_z, crystal_x, generatedCrystals);
                }
                else
                {
                    MakeCrystal1(crystal_z, crystal_x, generatedCrystals);
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




    void MakeCrystal2(int x, int z, int crystal2Count)
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


    void MakeCrystal1(int x, int z, int crystal1Count)
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
        HashSet<Vector2Int> wasHere = new HashSet<Vector2Int>();
        Queue<Vector2Int> coordinates = new Queue<Vector2Int>();
        Queue<Vector2Int> childCoordinates = new Queue<Vector2Int>();
        coordinates.Enqueue(new Vector2Int(0, 0));
        while (coordinates.Count > 0)
        {
            Vector2Int coordinate = coordinates.Dequeue();
            if (coordinate.x >= rows || coordinate.y >= columns)
            {
                continue;
            }

            GameObject pillar = game.Board.Pillars[coordinate.x, coordinate.y].PillarObject;
            if (pillar != null && !wasHere.Contains(coordinate))
            {
                Animator animator = pillar.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = true;
                    animator.speed = 4.0f;
                    animator.SetTrigger("StartAnimationState");
                }
                childCoordinates.Enqueue(new Vector2Int(coordinate.x + 1, coordinate.y));
                childCoordinates.Enqueue(new Vector2Int(coordinate.x, coordinate.y + 1));
                childCoordinates.Enqueue(new Vector2Int(coordinate.x + 1, coordinate.y + 1));
                wasHere.Add(coordinate);
            }


            if (coordinates.Count == 0)
            {
                coordinates = childCoordinates;
                childCoordinates = new Queue<Vector2Int>();
                yield return new WaitForSeconds(animationDelay);
            }
        }

        for (int i = 0; i < game.Board.CheapCrystals.Length; i++)
        {
            if (game.Board.CheapCrystals[i] != null)
            {
                Animator animator = game.Board.CheapCrystals[i].Crystal1Object.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = true;
                    animator.speed = 1.0f;
                    animator.SetTrigger("Crystal1GrowingTrigger");
                }
            }
        }

        for (int i = 0; i < game.Board.ExpensiveCrystals.Length; i++)
        {
            if (game.Board.ExpensiveCrystals[i] != null)
            {
                Animator animator = game.Board.ExpensiveCrystals[i].Crystal2Object.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = true;
                    animator.speed = 1.0f;
                    animator.SetTrigger("Crystal2GrowingTrigger");
                }
            }
        }

        for (int i = 0; i < game.Board.Bases.Length; i++)
        {
            if (game.Board.Bases[i] != null)
            {
                Animator animator = game.Board.Bases[i].BaseObject.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = true;
                    animator.speed = 1.0f;
                    animator.SetTrigger("Base" + (i + 1).ToString() + "BuildingTrigger");
                }
            }
        }

        Animator animatorP1 = game.Player1.PlayerObject.GetComponent<Animator>();
        if (animatorP1 != null)
        {
            animatorP1.enabled = true;
            animatorP1.speed = 1.0f;
            animatorP1.SetTrigger("UFO2LandingTrigger");
        }

        Animator animatorP2 = game.Player2.PlayerObject.GetComponent<Animator>();
        if (animatorP2 != null)
        {
            animatorP2.enabled = true;
            animatorP2.speed = 1.0f;
            animatorP2.SetTrigger("UFO1LandingTrigger");
        }

        // activate trigger on player 1
        // player1.GetComponent<Animator>().SetTrigger("UFO2LandingTrigger");
        // player2.GetComponent<Animator>().SetTrigger("UFO1LandingTrigger");

        // // activate trigger on base 1
        // basePlayer1.GetComponent<Animator>().SetTrigger("Base1BuildingTrigger");
        // basePlayer2.GetComponent<Animator>().SetTrigger("Base2BuildingTrigger");  
    }


}
