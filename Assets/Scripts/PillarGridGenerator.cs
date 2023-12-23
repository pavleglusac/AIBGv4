using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(1)]
public class PillarGridGenerator : MonoBehaviour
{

    public Game game;

    public GameObject pillarPrefab;

    public GameObject player1;
    public GameObject player2;

    public GameObject basePlayer1;
    public GameObject basePlayer2;

    public Transform cameraTransform;

    [HideInInspector] public int rows;
    [HideInInspector] public int columns;
    [HideInInspector] public float spacing;
    [HideInInspector] public float animationDelay;
    [HideInInspector] public int totalCheapCrystalCount;
    [HideInInspector] public int totalExpensiveCrystalCount;
    [HideInInspector] public int baseAreaLength;


    public static System.Random random = new System.Random();


    void Start()
    {
        game = Game.Instance;
        rows = game.rows;
        columns = game.columns;
        spacing = game.spacing;
        animationDelay = game.animationDelay;
        totalCheapCrystalCount = game.numOfCheapCrystalGroups;
        totalExpensiveCrystalCount = game.numOfExpensiveCrystalGroups;
        totalCheapCrystalCount = rows / 5;
        totalExpensiveCrystalCount = columns / 5;
        baseAreaLength = rows / 3;
        game.Board.Pillars = new Pillar[rows, columns];
        game.Board.Bases = new Base[2];
        GenerateGrid();
        MakeBase();
        MakePlayers();

        game.Board.Pillars[rows - 1, 0].PillarState = PillarState.Player1;
        game.Board.Pillars[0, columns - 1].PillarState = PillarState.Player2;
        StartCoroutine(StartAnimations());
    }

    void MakeBase()
    {
        int i = rows - 1;
        int j = 0;
        game.Board.Pillars[i, j].PillarState = PillarState.BasePlayer1;
        game.Board.Pillars[i, j].LastState = PillarState.BasePlayer1;
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
        j = columns - 1;
        game.Board.Pillars[i, j].PillarState = PillarState.BasePlayer2;
        game.Board.Pillars[i, j].LastState = PillarState.BasePlayer2;
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

    void MakePlayers()
    {
        int i = rows - 1;
        int j = 0;
        GameObject playerObject1 = Instantiate(player1, new Vector3(i * spacing, 21, j * spacing), Quaternion.identity, this.transform);
        playerObject1.AddComponent<Player>();
        playerObject1.GetComponent<Player>().PlayerObject = playerObject1;
        game.Player1 = playerObject1.GetComponent<Player>();
        //game.Player1.X = i;
        //game.Player1.Z = j;
        game.Player1.SetPosition(game.Board.Pillars[i, j]);
        game.Player1.SetupPlayer("Crni Cerak");

        i = 0;
        j = columns - 1;
        GameObject playerObject2 = Instantiate(player2, new Vector3(i * spacing, 21, j * spacing), Quaternion.identity, this.transform);
        playerObject2.AddComponent<Player>();
        playerObject2.GetComponent<Player>().PlayerObject = playerObject2;
        game.Player2 = playerObject2.GetComponent<Player>();
        game.Player2.SetPosition(game.Board.Pillars[i, j]);
        game.Player2.SetupPlayer("Pupoljci");

        game.UpdateAllPlayerStats();
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
    }

}