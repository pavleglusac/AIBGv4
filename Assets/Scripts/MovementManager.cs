using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public float speed = 5.0f;

    public GameObject player;

    public List<(int direction, int steps)> directions = new List<(int direction, int steps)>();

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float stepDuration = 0.2f;

    void Update()
    {
        if (!isMoving && directions.Count > 0)
        {
            StartCoroutine(ProcessDirections());
        }
    }

    public void AddMovement(int direction, int steps)
    {
        Debug.Log("Adding movement: " + direction + " " + steps);
        directions.Add((direction, steps));
    }

    private Vector3 GetDirectionVector(int direction)
    {
        switch (direction)
        {
            case 0: return new Vector3(0, 0, -1.2f);
            case 1: return new Vector3(-1.2f, 0, 0f);
            case 2: return new Vector3(0, 0, 1.2f);
            case 3: return new Vector3(1.2f, 0, 0f);
            default:
                Debug.LogError("Invalid direction: " + direction);
                return Vector3.zero;
        }
    }

    private IEnumerator ProcessDirections()
    {
        isMoving = true;
        
        while (directions.Count > 0)
        {
            var movement = directions[0];
            directions.RemoveAt(0);
            yield return StartCoroutine(MoveInDirection(movement.direction, movement.steps));
        }
        
        isMoving = false;
    }

    private IEnumerator MoveInDirection(int direction, int steps)
    {
        Vector3 directionVector = GetDirectionVector(direction);
        
        for (int i = 0; i < steps; i++)
        {
            Vector3 startPosition = player.transform.position;
            Vector3 targetPosition = startPosition + directionVector;
            float elapsedTime = 0f;
            
            while (elapsedTime < stepDuration)
            {
                player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / stepDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            player.transform.position = targetPosition;  // Ensure the final position is accurate

        }
    }
}