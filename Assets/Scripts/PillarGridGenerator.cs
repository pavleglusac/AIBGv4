using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarGridGenerator : MonoBehaviour
{

    public Game game;
    public GameObject pillarPrefab;

    public GameObject player1;
    public GameObject player2;
    
    public Transform cameraTransform;

    public int rows = 12;
    public int columns = 12;
    public float spacing = 2.0f;

    public float animationDelay = 0.01f;

    void Start()
    {
        game = Game.Instance;
        game.Board.Pillars = new Pillar[rows, columns];
        GenerateGrid();
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

    void GenerateGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
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

        // activate trigger on player 1
        player1.GetComponent<Animator>().SetTrigger("UFO2LandingTrigger");
        player2.GetComponent<Animator>().SetTrigger("UFO1LandingTrigger");
    }
}