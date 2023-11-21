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
    public int rows;
    public int columns;
    public float spacing;
    public float animationDelay;
    public float totalCrystal1Count;
    public float totalCrystal2Count;
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
        totalCrystal1Count = rows / 5;
        totalCrystal2Count = columns / 5;
        baseAreaLength = rows / 3 + 1;
        // print all these 5 variables
        Debug.Log("rows: " + rows + " columns: " + columns + " spacing: " + spacing + " animationDelay: " + animationDelay + " totalCrystal1Count: " + totalCrystal1Count + " totalCrystal2Count: " + totalCrystal2Count + " baseAreaLength: " + baseAreaLength);
        game.Board.Crystals1 = new Crystal1[(int)(totalCrystal1Count * 3 * 2)];
        game.Board.Crystals2 = new Crystal2[(int)(totalCrystal2Count * 3 * 2)];
        GenerateCrystals1();
        GenerateCrystals2();
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

    void GenerateCrystals2()
    {
        Tuple<int, int>[] crystal2Coordinates = new Tuple<int, int>[int.Parse(totalCrystal2Count.ToString())];
        List<Tuple<int, int>> crystals2Cordinates = new List<Tuple<int, int>>();
        Boolean up = true;
        for (int i = 0; i < crystal2Coordinates.Length; i++)
        {

            Tuple<int, int> coordinates = GenerateCoordinates(up);
            int x = coordinates.Item1;
            int z = coordinates.Item2;
            up = !up;

            crystal2Coordinates[i] = new Tuple<int, int>(x, z);
            for (int j = 0; j < i; j++)
            {
                if ((crystal2Coordinates[j].Item1 == x && crystal2Coordinates[j].Item2 == z) || (crystal2Coordinates[j].Item1 == z && crystal2Coordinates[j].Item2 == x))
                {
                    i--;
                    break;
                }
            }
            crystals2Cordinates.AddRange(GenerateCrystal2Group(x, z, crystals2Cordinates));
        }

        int generatedCrystals2 = 0;
        for (int i = 0; i < crystals2Cordinates.Count; i++)
        {
            int x = crystals2Cordinates[i].Item1;
            int z = crystals2Cordinates[i].Item2;
            if (game.Board.Pillars[x, z].PillarState == PillarState.Empty && game.Board.Pillars[z, x].PillarState == PillarState.Empty)
            {
                MakeCrystal2(x, z, generatedCrystals2);
                generatedCrystals2++;
                MakeCrystal2(z, x, generatedCrystals2);
                generatedCrystals2++;
            }
        }

    }

    

    List<Tuple<int, int>> GenerateCrystal2Group(int x, int z, List<Tuple<int, int>> rocks)
    {
        List<Tuple<int, int>> crystals2Coordinates = new List<Tuple<int, int>>();
        for (int i = 0; i < 2; i++)
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

            if(!CheckIfCoordinatesAreValid(xCoordinate, zCoordinate) || rocks.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)) || crystals2Coordinates.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)))
            {
                i--;
                continue;
            }

            if (game.Board.Pillars[xCoordinate, zCoordinate].PillarState == PillarState.Empty)
            {
                crystals2Coordinates.Add(new Tuple<int, int>(xCoordinate, zCoordinate));
            }

        }

        return crystals2Coordinates;
    }


    void MakeCrystal2(int x, int z, int crystal2Count)
    {
        game.Board.Pillars[x, z].PillarState = PillarState.Crystal2;
        GameObject crystal2Object = Instantiate(crystal2Prefab, new Vector3(x * spacing, -50, z * spacing), Quaternion.identity, this.transform);
        crystal2Object.AddComponent<Crystal2>();
        crystal2Object.GetComponent<Crystal2>().Crystal2Object = crystal2Object;
        crystal2Object.GetComponent<Crystal2>().SetPosition(game.Board.Pillars[x, z]);
        game.Board.Crystals2[crystal2Count] = crystal2Object.GetComponent<Crystal2>();
        // set tree enabled to false
        // game.Board.Trees[treeCount].TreeObject.SetActive(false);

        Animator animator = crystal2Object.GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = false;
        }

    }

    void GenerateCrystals1()
    {
        // make array of tuples cordinates
        Tuple<int, int>[] crystal1groupCoordinates = new Tuple<int, int>[int.Parse(totalCrystal1Count.ToString())];
        List<Tuple<int, int>> crystal1Cordinates = new List<Tuple<int, int>>();
        Boolean up = true;
        for(int i = 0; i < crystal1groupCoordinates.Length; i++)
        {

            Tuple<int, int> coordinates = GenerateCoordinates(up);
            int x = coordinates.Item1;
            int z = coordinates.Item2;
            up = !up;

            crystal1groupCoordinates[i] = new Tuple<int, int>(x, z);
            for (int j = 0; j < i; j++)
            {
                if ((crystal1groupCoordinates[j].Item1 == x && crystal1groupCoordinates[j].Item2 == z) || (crystal1groupCoordinates[j].Item1 == z && crystal1groupCoordinates[j].Item2 == x))
                {
                    i--;
                    break;
                }
            }

            crystal1Cordinates.AddRange(GenerateCrystal1Groups(x, z, crystal1Cordinates));

        }

        // for all cordinates
        int generatedCrystals1 = 0;
        for (int i = 0; i < crystal1Cordinates.Count; i++)
        {
            int x = crystal1Cordinates[i].Item1;
            int z = crystal1Cordinates[i].Item2;
            if (game.Board.Pillars[x, z].PillarState == PillarState.Empty && game.Board.Pillars[z, x].PillarState == PillarState.Empty)
            {
                MakeCrystal1(x, z, generatedCrystals1);
                generatedCrystals1++;
                MakeCrystal1(z, x, generatedCrystals1);
                generatedCrystals1++;
            }
        }

        

    }

    List<Tuple<int, int>> GenerateCrystal1Groups(int x, int z, List<Tuple<int, int>> crystals1)
    {
        List<Tuple<int, int>> crystals1Coordinates = new List<Tuple<int, int>>();
        // make 2 or 3 trees but stay in bounds and do not overlap with other trees
        for (int i = 0; i < 3; i++)
        {
            // generate random x and z up down left or right
            int xCoordinate = x;
            int zCoordinate = z;
            
            if(i != 0)
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


            if (!CheckIfCoordinatesAreValid(xCoordinate, zCoordinate) || crystals1.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)) || crystals1Coordinates.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)))
            {
                i--;
                continue;
            }

            if (game.Board.Pillars[xCoordinate, zCoordinate].PillarState == PillarState.Empty)
            {
                crystals1Coordinates.Add(new Tuple<int, int>(xCoordinate, zCoordinate));
            }
        }

        return crystals1Coordinates;
    }

    void MakeCrystal1(int x, int z, int crystal1Count)
    {
        game.Board.Pillars[x, z].PillarState = PillarState.Crystal1;
        GameObject crystal1Object = Instantiate(crystal1Prefab, new Vector3(x * spacing, -50, z * spacing), Quaternion.identity, this.transform);
        crystal1Object.AddComponent<Crystal1>();
        crystal1Object.GetComponent<Crystal1>().Crystal1Object = crystal1Object;
        game.Board.Crystals1[crystal1Count] = crystal1Object.GetComponent<Crystal1>();
        game.Board.Crystals1[crystal1Count].SetPosition(game.Board.Pillars[x, z]);

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

        for (int i = 0; i < game.Board.Crystals1.Length; i++)
        {
            if (game.Board.Crystals1[i] != null)
            {
                Animator animator = game.Board.Crystals1[i].Crystal1Object.GetComponent<Animator>();
                if(animator != null)
                {
                    animator.enabled = true;
                    animator.speed = 1.0f;
                    animator.SetTrigger("Crystal1GrowingTrigger");
                }
            }
        }

        for (int i = 0; i < game.Board.Crystals2.Length; i++)
        {
            if (game.Board.Crystals2[i] != null)
            {
                Animator animator = game.Board.Crystals2[i].Crystal2Object.GetComponent<Animator>();
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
