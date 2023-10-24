using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(1)]
public class PillarGridGenerator : MonoBehaviour
{

    public Game game;

    public GameObject pillarPrefab;
    public GameObject treePrefab;
    public GameObject rockPrefab;

    public GameObject player1;
    public GameObject player2;

    public GameObject basePlayer1;
    public GameObject basePlayer2;
    
    public Transform cameraTransform;

    public int rows = 12;
    public int columns = 12;
    public float spacing = 2.0f;

    public float animationDelay = 0.01f;

    public float totalForestCount = 2;
    public float totalRockCount = 3;

    public static System.Random random = new System.Random();


    void Start()
    {
        game = Game.Instance;
        game.Board.Pillars = new Pillar[rows, columns];
        game.Board.Trees = new Tree[12];
        game.Board.Rocks = new Rock[12];
        game.Board.Bases = new Base[2];
        GenerateGrid();
        GenerateTrees();
        GenerateRocks();
        MakeBase();
        game.Board.Pillars[11, 0].PillarState = PillarState.Player1;
        game.Board.Pillars[0, 11].PillarState = PillarState.Player2;
        StartCoroutine(StartAnimations());
        // get first child of player 1 and player 2
        Player player1Object = player1.transform.GetChild(0).GetComponent<Player>();
        Player player2Object = player2.transform.GetChild(0).GetComponent<Player>();
        
        game.Player1 = player1Object;
        game.Player1.PlayerObject = player1;
        game.Player2 = player2Object;
        game.Player2.PlayerObject = player2;

        game.Player1.X = 11;
        game.Player1.Z = 0;
        game.Player2.X = 0;
        game.Player2.Z = 11;

    }

    public static Tuple<int, int> GenerateCoordinates(bool up)
    {
        int x, z;
        if (up)
        {    
            do
            {
                x = random.Next(1, 8);
                z = random.Next(1, 5); 
                Debug.Log("x: " + x + " z: " + z);
            } while (!(x > z)); 
        }
        else
        {
            do
            {
                x = random.Next(7, 12);
                z = random.Next(4, 11); 
            } while (!(x > z));
        }
        return new Tuple<int, int>(x, z);

    }

    bool CheckIfCoordinatesAreValid(int x, int z)
    {
        Debug.Log("x: " + x + " z: " + z);
        if (8 <= x && x <= 11 && 0 <= z && z <= 3)
        {
            return false;
        }
        if (x < 0 || x >= rows || z < 0 || z >= columns)
        {
            return false;
        }
        return true;
    }

    void GenerateRocks()
    {
        Tuple<int, int>[] rockCoordinates = new Tuple<int, int>[int.Parse(totalRockCount.ToString()) - 1];
        List<Tuple<int, int>> rocksCordinates = new List<Tuple<int, int>>();
        Boolean up = true;
        for (int i = 0; i < rockCoordinates.Length; i++)
        {

            Tuple<int, int> coordinates = GenerateCoordinates(up);
            int x = coordinates.Item1;
            int z = coordinates.Item2;
            up = !up;

            rockCoordinates[i] = new Tuple<int, int>(x, z);
            for (int j = 0; j < i; j++)
            {
                if ((rockCoordinates[j].Item1 == x && rockCoordinates[j].Item2 == z) || (rockCoordinates[j].Item1 == z && rockCoordinates[j].Item2 == x))
                {
                    i--;
                    break;
                }
            }
            rocksCordinates.AddRange(GenerateRockGroup(x, z, rocksCordinates));
        }

        int generatedRocks = 0;
        for (int i = 0; i < rocksCordinates.Count; i++)
        {
            int x = rocksCordinates[i].Item1;
            int z = rocksCordinates[i].Item2;
            if (game.Board.Pillars[x, z].PillarState == PillarState.Empty && game.Board.Pillars[z, x].PillarState == PillarState.Empty)
            {
                MakeRock(x, z, generatedRocks);
                generatedRocks++;
                MakeRock(z, x, generatedRocks);
                generatedRocks++;
            }
        }

    }

    

    List<Tuple<int, int>> GenerateRockGroup(int x, int z, List<Tuple<int, int>> rocks)
    {
        List<Tuple<int, int>> rocksCoordinates = new List<Tuple<int, int>>();
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

            if(!CheckIfCoordinatesAreValid(xCoordinate, zCoordinate) || rocks.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)) || rocksCoordinates.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)))
            {
                i--;
                continue;
            }

            if (game.Board.Pillars[xCoordinate, zCoordinate].PillarState == PillarState.Empty)
            {
                rocksCoordinates.Add(new Tuple<int, int>(xCoordinate, zCoordinate));
            }

        }

        return rocksCoordinates;
    }


    void MakeRock(int x, int z, int rockCount)
    {
        game.Board.Pillars[x, z].PillarState = PillarState.Rock;
        GameObject rockObject = Instantiate(rockPrefab, new Vector3(x * spacing, -50, z * spacing), Quaternion.identity, this.transform);
        rockObject.AddComponent<Rock>();
        rockObject.GetComponent<Rock>().RockObject = rockObject;
        game.Board.Rocks[rockCount] = rockObject.GetComponent<Rock>();
        game.Board.Rocks[rockCount].X = x;
        game.Board.Rocks[rockCount].Z = z;
        // set tree enabled to false
        // game.Board.Trees[treeCount].TreeObject.SetActive(false);

        Animator animator = rockObject.GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = false;
        }

    }

    void GenerateTrees()
    {
        // make array of tuples cordinates
        Tuple<int, int>[] forestCoordinates = new Tuple<int, int>[int.Parse(totalForestCount.ToString()) - 1];
        List<Tuple<int, int>> treesCordinates = new List<Tuple<int, int>>();
        Boolean up = true;
        for(int i = 0; i < forestCoordinates.Length; i++)
        {

            Tuple<int, int> coordinates = GenerateCoordinates(up);
            int x = coordinates.Item1;
            int z = coordinates.Item2;
            up = !up;

            forestCoordinates[i] = new Tuple<int, int>(x, z);
            for (int j = 0; j < i; j++)
            {
                if ((forestCoordinates[j].Item1 == x && forestCoordinates[j].Item2 == z) || (forestCoordinates[j].Item1 == z && forestCoordinates[j].Item2 == x))
                {
                    i--;
                    break;
                }
            }

            treesCordinates.AddRange(GenerateForest(x, z, treesCordinates));

        }

        // for all cordinates
        int generatedTrees = 0;
        for (int i = 0; i < treesCordinates.Count; i++)
        {
            int x = treesCordinates[i].Item1;
            int z = treesCordinates[i].Item2;
            if (game.Board.Pillars[x, z].PillarState == PillarState.Empty && game.Board.Pillars[z, x].PillarState == PillarState.Empty)
            {
                MakeTree(x, z, generatedTrees);
                generatedTrees++;
                MakeTree(z, x, generatedTrees);
                generatedTrees++;
            }
        }

        

    }

    List<Tuple<int, int>> GenerateForest(int x, int z, List<Tuple<int, int>> trees)
    {
        List<Tuple<int, int>> treesCoordinates = new List<Tuple<int, int>>();
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


            if (!CheckIfCoordinatesAreValid(xCoordinate, zCoordinate) || trees.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)) || treesCoordinates.Contains(new Tuple<int, int>(xCoordinate, zCoordinate)))
            {
                i--;
                continue;
            }

            if (game.Board.Pillars[xCoordinate, zCoordinate].PillarState == PillarState.Empty)
            {
                treesCoordinates.Add(new Tuple<int, int>(xCoordinate, zCoordinate));
            }
        }

        return treesCoordinates;
    }

    void MakeTree(int x, int z, int treeCount)
    {
        game.Board.Pillars[x, z].PillarState = PillarState.Tree;
        GameObject treeObject = Instantiate(treePrefab, new Vector3(x * spacing, -50, z * spacing), Quaternion.identity, this.transform);
        treeObject.AddComponent<Tree>();
        treeObject.GetComponent<Tree>().TreeObject = treeObject;
        game.Board.Trees[treeCount] = treeObject.GetComponent<Tree>();
        game.Board.Trees[treeCount].X = x;
        game.Board.Trees[treeCount].Z = z;
        // set tree enabled to false
        // game.Board.Trees[treeCount].TreeObject.SetActive(false);

        Animator animator = treeObject.GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    void MakeBase()
    {
        int i = 11;
        int j = 0;
        game.Board.Pillars[i, j].PillarState = PillarState.BasePlayer1;
        GameObject baseObject = Instantiate(basePlayer1, new Vector3(i * spacing, -50, j * spacing), Quaternion.identity, this.transform);
        baseObject.AddComponent<Base>();
        baseObject.GetComponent<Base>().BaseObject = baseObject;
        game.Board.Bases[0] = baseObject.GetComponent<Base>();
        game.Board.Bases[0].X = i;
        game.Board.Bases[0].Z = j;

        Animator animator = baseObject.GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = false;
        }

        i = 0;
        j = 11;
        game.Board.Pillars[i, j].PillarState = PillarState.BasePlayer2;
        GameObject baseObject2 = Instantiate(basePlayer2, new Vector3(i * spacing, -50, j * spacing), Quaternion.identity, this.transform);
        baseObject2.AddComponent<Base>();
        baseObject2.GetComponent<Base>().BaseObject = baseObject2;
        game.Board.Bases[1] = baseObject2.GetComponent<Base>();
        game.Board.Bases[1].X = i;
        game.Board.Bases[1].Z = j;

        Animator animator2 = baseObject2.GetComponent<Animator>();

        if (animator2 != null)
        {
            animator2.enabled = false;
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                MakePillar(i, j);
            }
        }

    }

    void MakePillar(int i, int j)
    {
        Vector3 position = new Vector3(i * spacing, 0, j * spacing);
        GameObject pillarObject = Instantiate(pillarPrefab, position, Quaternion.identity, this.transform);
        pillarObject.AddComponent<Pillar>();
        pillarObject.GetComponent<Pillar>().PillarObject = pillarObject;
        game.Board.Pillars[i, j] = pillarObject.GetComponent<Pillar>();
        game.Board.Pillars[i, j].X = i;
        game.Board.Pillars[i, j].Z = j;

        Animator animator = pillarObject.GetComponent<Animator>();

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

        for (int i = 0; i < game.Board.Trees.Length; i++)
        {
            if (game.Board.Trees[i] != null)
            {
                Animator animator = game.Board.Trees[i].TreeObject.GetComponent<Animator>();
                if(animator != null)
                {
                    animator.enabled = true;
                    animator.speed = 1.0f;
                    animator.SetTrigger("TreeGrowingTrigger");
                }
            }
        }

        for (int i = 0; i < game.Board.Rocks.Length; i++)
        {
            if (game.Board.Rocks[i] != null)
            {
                Animator animator = game.Board.Rocks[i].RockObject.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = true;
                    animator.speed = 1.0f;
                    animator.SetTrigger("RockGrowingTrigger");
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

        // activate trigger on player 1
        player1.GetComponent<Animator>().SetTrigger("UFO2LandingTrigger");
        player2.GetComponent<Animator>().SetTrigger("UFO1LandingTrigger");

        // // activate trigger on base 1
        // basePlayer1.GetComponent<Animator>().SetTrigger("Base1BuildingTrigger");
        // basePlayer2.GetComponent<Animator>().SetTrigger("Base2BuildingTrigger");

        
    }

    

    
}