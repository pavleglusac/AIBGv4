using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;



// inherit from command
public class MoveCommand : MonoBehaviour, IEnergySpendingCommand
{
    public int Direction { get; set; }
    public int Count { get; set; }
    public Player Player { get; set; }
    Pillar TargetPillar { get; set; }
    List<Pillar>  Path { get; set; }
    private int EnergyCost;
    public bool isMoving { get; set; } = false;
    public bool isDone { get; set; } = false;
    private float stepDuration = 0.2f;
    private bool isCoroutineRunning = false;

    public MoveCommand Initialize(int x, int z)
    {
        Player = Game.Instance.GetCurrentPlayer();
        TargetPillar = Game.Instance.Board.Pillars[x, z];
        
        EnergyCost = int.Parse(PlayerPrefs.GetString("movement_cost"));
        return this;
    }

    public void Execute()
    {

        Pillar prev = Player.Position;
        Count = Math.Abs(TargetPillar.X - Player.X) + Math.Abs(TargetPillar.Z - Player.Z);
        Direction = GetDirection(prev, TargetPillar);

        Player.Position.PillarState = Player.Position.LastState;
        TargetPillar.LastState = TargetPillar.PillarState;
        TargetPillar.PillarState = Game.Instance.FirstPlayerTurn ? PillarState.Player1 : PillarState.Player2;
        Player.SetPosition(TargetPillar);

        isMoving = true;
        Player.DecreaseEnergy(GetEnergyCost());
        Game.Instance.DisplayMessage = "Moved to position [" + Player.Position.X + "," + Player.Position.Z + "]";
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
        isDone = true;
        isMoving = false;
        isCoroutineRunning = false;
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

    private static int GetDirection(Pillar prev, Pillar current)
    {
        if (prev.X == current.X)
        {
            return prev.Z > current.Z ? 0 : 2;
        }
        else if (prev.Z == current.Z)
        {
            return prev.X > current.X ? 1 : 3;
        }
        return -1;
    }

    private static bool OutOfBounds(Pillar pillar, Player player)
    {
        int stepX = Math.Abs(pillar.X - player.X);
        int stepZ = Math.Abs(pillar.Z - player.Z);

        int newX = player.X - stepX;
        int newZ = player.Z - stepZ;

        newX = (pillar.X > player.X) ? newX : player.X + stepX;
        newZ = (pillar.Z > player.Z) ? newZ : player.Z + stepZ;
        int boardSize = int.Parse(PlayerPrefs.GetString("board_size")) - 1;

        return newX < 0 || newX > boardSize || newZ < 0 || newZ > boardSize;
    }

    private static Pillar ChangePillarsBasedOnDaze(Pillar pillar, Player player)
    {
        int stepX = Math.Abs(pillar.X - player.X);
        int stepZ = Math.Abs(pillar.Z - player.Z);

        int newX = player.X - stepX;
        int newZ = player.Z - stepZ;

        newX = (pillar.X > player.X) ? newX : player.X + stepX;
        newZ = (pillar.Z > player.Z) ? newZ : player.Z + stepZ;

        return Game.Instance.Board.Pillars[newX, newZ];
    }

    public bool IsDone()
    {
        return isDone;
    }

    public override string ToString()
    {
        return "MoveCommand: " + Player.FirstPlayer + " " + Direction + " " + Count;
    }

    public bool CanExecute()
    {
        if (Player.IsDazed())
        {
            if (OutOfBounds(TargetPillar, Player)) {
                Game.Instance.DisplayMessage = "Move out of bounds!";
                return false;
            }
            TargetPillar = ChangePillarsBasedOnDaze(TargetPillar, Player);
        }

        if (Player.Position.X == TargetPillar.X && Player.Position.Z == TargetPillar.Z)
        {
            Game.Instance.DisplayMessage = "Cannot stay on the same place!";
            return false;
        }

        Path = Algorithms.FindPath(Game.Instance.Board, Player.Position, TargetPillar);

        if (Path == null)
        {
            Game.Instance.DisplayMessage = "You can only move horizontally and vertically!";
            return false;
        }
        if (Path.Count == 0 || !Path.Contains(TargetPillar))
        {
            Game.Instance.DisplayMessage = "Obstacle on the way!";
            return false;
        }

        if (Player.Energy < GetEnergyCost())
        {
            Game.Instance.DisplayMessage = "You don't have enough energy to move!";
            return false;
        }
        return true;
    }

    public int GetEnergyCost()
    {
        return (Player.Bag.GetWeight() + EnergyCost) * Count;
    }

}
