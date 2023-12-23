using System;
using System.Collections;
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
    [HideInInspector] public int numOfCheapCrystalGroups;
    [HideInInspector] public int numOfExpensiveCrystalGroups;
    public int baseAreaLength;
    public static System.Random random = new System.Random();
    
    // Start is called before the first frame update
    void Start()
    {
        game = Game.Instance;
        rows = game.rows;
        columns = game.columns;
        spacing = game.spacing;
        animationDelay = game.animationDelay;
        numOfCheapCrystalGroups = game.numOfCheapCrystalGroups;
        numOfExpensiveCrystalGroups = game.numOfExpensiveCrystalGroups;
        // totalCheapCrystalCount = rows / 5;
        // totalExpensiveCrystalCount = columns / 5;
        baseAreaLength = rows / 3 + 1;
        // print all these 5 variables
        Debug.Log("rows: " + rows + " columns: " + columns + " spacing: " + spacing + " animationDelay: " + animationDelay + " totalCrystal1Count: " + numOfCheapCrystalGroups + " totalCrystal2Count: " + numOfExpensiveCrystalGroups + " baseAreaLength: " + baseAreaLength);
        game.Board.CheapCrystals = new CheapCrystal[(int)(numOfCheapCrystalGroups * 3 * 2)]; // 3 crystals per group, 2 groups one for each side
        game.Board.ExpensiveCrystals = new ExpensiveCrystal[(int)(numOfExpensiveCrystalGroups * 3 * 2)]; // 3 crystals per group, 2 groups one for each side
        GenerateCrystals(false);
        GenerateCrystals(true);
        StartCoroutine(StartAnimations());

    }

    public Tuple<int, int> GenerateCoordinates(bool up)
    {
        int x, z;
        if (up)
        {    
            do
            {
                x = random.Next(1, rows - baseAreaLength);
                z = random.Next(1, baseAreaLength + 1); 
                //Debug.Log("x: " + x + " z: " + z);
            } while (!(x > z)); 
        }
        else
        {
            do
            {
                x = random.Next(rows - baseAreaLength - 1, rows);
                z = random.Next(baseAreaLength, columns - 1); 
            } while (!(x > z));
        }
        return new Tuple<int, int>(x, z);

    }

    bool CheckIfCoordinatesAreValid(int x, int z)
    {
        //Debug.Log("x: " + x + " z: " + z);
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


    void GenerateCrystals(bool isExpensive)
    {
        int numOfGroups = isExpensive ? int.Parse(numOfExpensiveCrystalGroups.ToString()) : numOfCheapCrystalGroups;
        Tuple<int, int>[] groupCoordinates = new Tuple<int, int>[numOfGroups];
        List<Tuple<int, int>> crystalsCoordinates = new List<Tuple<int, int>>();
        bool up = true;

        for (int i = 0; i < groupCoordinates.Length; i++)
        {
            Tuple<int, int> coordinates = GenerateCoordinates(up);
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

            crystalsCoordinates.AddRange(GenerateCrystalGroup(x, z, crystalsCoordinates, isExpensive));
        }

        int generatedCrystals = 0;
        for (int i = 0; i < crystalsCoordinates.Count; i++)
        {
            int x = crystalsCoordinates[i].Item1;
            int z = crystalsCoordinates[i].Item2;
            if (game.Board.Pillars[x, z].PillarState == PillarState.Empty && game.Board.Pillars[z, x].PillarState == PillarState.Empty)
            {
                if (isExpensive)
                {
                    MakeCrystal2(x, z, generatedCrystals);
                }
                else
                {
                    MakeCrystal1(x, z, generatedCrystals);
                }
                generatedCrystals++;

                if (isExpensive)
                {
                    MakeCrystal2(z, x, generatedCrystals);
                }
                else
                {
                    MakeCrystal1(z, x, generatedCrystals);
                }
                generatedCrystals++;
            }
        }
    }


    List<Tuple<int, int>> GenerateCrystalGroup(int x, int z, List<Tuple<int, int>> existingCrystals, bool isExpensive)
    {
        List<Tuple<int, int>> newCrystalsCoordinates = new List<Tuple<int, int>>();
        int groupSize = isExpensive ? 3 : 3; // Adjust size if needed for expensive/non-expensive groups

        for (int i = 0; i < groupSize; i++)
        {
            int xCoordinate = x;
            int zCoordinate = z;

            if (i != 0)
            {
                int direction = Random.Range(0, 4);
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
        while(coordinates.Count > 0)
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
                if(animator != null)
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
