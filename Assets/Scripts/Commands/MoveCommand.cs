using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 


// inherit from command
public class MoveCommand : MonoBehaviour, IEnergySpendingCommand
{
    public int Direction { get; set; }
    public int Count { get; set; }
    public Player Player { get; set; }
    public bool isMoving { get; set; } = false;
    public bool isDone { get; set; } = false;
    private float stepDuration = 0.2f;
    private bool isCoroutineRunning = false;
    private int energyCost;

    public MoveCommand Initialize(Player player, int direction, int count)
    {
        Direction = direction;
        Count = count;
        Player = player;
        energyCost = int.Parse(PlayerPrefs.GetString("movement_cost"));
        return this;
    }

    public void Execute()
    {
        isMoving = true;
        Player.DecreaseEnergy(GetEnergyCost());
        Game.Instance.SwitchPlayersAndDecreaseStats();
    }

    public void Update()
    {
        if (isMoving && !isCoroutineRunning)
        {
            isCoroutineRunning = true;
            StartCoroutine(ProcessDirections());
        }
    }

    private IEnumerator ProcessDirections()
    {
        isCoroutineRunning = true;
        yield return StartCoroutine(MoveInDirection(Direction, Count));
        isMoving = false;
        isCoroutineRunning = false;
        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }

    private Vector3 GetDirectionVector(int direction)
    {
        switch (direction)
        {
            case 0: return new Vector3(0, 0, -1f);
            case 1: return new Vector3(-1f, 0, 0f);
            case 2: return new Vector3(0, 0, 1f);
            case 3: return new Vector3(1f, 0, 0f);
            default:
                return Vector3.zero;
        }
    }


    private IEnumerator MoveInDirection(int direction, int steps)
    {
        Vector3 directionVector = GetDirectionVector(direction);
        
        for (int i = 0; i < steps; i++)
        {
            Vector3 startPosition = Player.PlayerObject.transform.position;
            Vector3 targetPosition = startPosition + directionVector;
            float elapsedTime = 0f;
            
            while (elapsedTime < stepDuration)
            {
                Player.PlayerObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / stepDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Player.PlayerObject.transform.position = targetPosition;

        }
    }

    public override string ToString()
    {
        return "MoveCommand: " + Player.FirstPlayer + " " + Direction + " " + Count;
    }

    public bool CanExecute()
    {
        return Player.Energy >= GetEnergyCost();
    }

    public int GetEnergyCost()
    {
        return (Player.Bag.GetWeight() + energyCost) * Count;
    }

}
