using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;


public class CommandParser : MonoBehaviour
{
    public CommandManager CommandManager {get; set;}
    public Game Game {get; set;}
    string movePattern = @"^move (-?\d+) (-?\d+)$";
    string minePattern = @"^mine (-?\d+) (-?\d+)$";
    string buildPattern = @"^build (-?\d+) (-?\d+)$";
    string conversionsPattern = @"^conv (\d+) exp (\d+) cheap to coins, (\d+) exp (\d+) cheap to energy, (\d+) exp (\d+) cheap to xp$";
    string restPattern = @"^rest";
    string shopPattern = @"^shop (freeze|backpack|daze)$";

    private readonly Queue<Action> mainThreadActions = new Queue<Action>();

    void Update()
    {
        while (mainThreadActions.Count > 0)
        {
            Action action = null;

            lock (mainThreadActions)
            {
                if (mainThreadActions.Count > 0)
                {
                    action = mainThreadActions.Dequeue();
                }
            }

            action?.Invoke();
        }
    }
    

    public void ParseCommand(string command) 
    {

        if (Regex.IsMatch(command, movePattern))
        {
            HandleMovement(command);
        }
        else if (Regex.IsMatch(command, minePattern))
        {
            HandleMine(command);
        }
        else if (Regex.IsMatch(command, buildPattern))
        {
            HandleBuild(command);
        }
        else if (Regex.IsMatch(command, conversionsPattern))
        {
            HandleConversionsAtBase(command);
        }
        else if (Regex.IsMatch(command, restPattern))
        {
            HandleResting();
        }
        else if (Regex.IsMatch(command, shopPattern))
        {
            var match = Regex.Match(command, shopPattern);
            var choice = match.Groups[1].Value;
            switch (choice)
            {
                case "freeze":
                    HandleFreeze();
                    break;
                case "backpack":
                    HandleBackpack();
                    break;
                case "daze":
                    HandleDaze();
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input, command not recognised!");
            InvalidTurnHandling();
        }

    }


    private void HandleMovement(string command) 
    {
        var match = Regex.Match(command, movePattern);
        if (match.Success)
        {
            int direction = int.Parse(match.Groups[1].Value);
            int count = int.Parse(match.Groups[2].Value);

            mainThreadActions.Enqueue(() => {
                GameObject commandObject = new GameObject("MoveCommandObject");
                MoveCommand moveCommandInstance = commandObject.AddComponent<MoveCommand>();
                moveCommandInstance.Initialize(Game.Instance.GetCurrentPlayer(), direction, count);
                Game.Instance.CommandManager.AddCommand(moveCommandInstance);
            });
            
            UnityEngine.Debug.Log($"MOVE {direction} {count}");
        }
        else
        {
            Console.WriteLine("Invalid move command.");
        }
    }

    private void HandleMine(string command) 
    {
        var match = Regex.Match(command, minePattern);
        if (match.Success)
        {
            int x = int.Parse(match.Groups[1].Value);
            int z = int.Parse(match.Groups[2].Value);

            mainThreadActions.Enqueue(() => {
                Actions.Mine(x, z);
            });
            
            UnityEngine.Debug.Log($"MINE {x} {z}");
        }
        else
        {
            Console.WriteLine("Invalid move command.");
        }
    }

    private void HandleBuild(string command) 
    {
        var match = Regex.Match(command, buildPattern);
        if (match.Success)
        {
            int x = int.Parse(match.Groups[1].Value);
            int z = int.Parse(match.Groups[2].Value);

            mainThreadActions.Enqueue(() => {
                Actions.BuildHouse(Game.GetCurrentPlayer(), Game.Instance.Board.Pillars[x, z]);
            });
            
            UnityEngine.Debug.Log($"MINE {x} {z}");
        }
        else
        {
            Console.WriteLine("Invalid move command.");
        }
    }

    private void HandleConversionsAtBase(string command) 
    {

    }

    private void HandleResting() 
    {
        var match = Regex.Match(command, restPattern);
        if (match.Success)
        {

            mainThreadActions.Enqueue(() => {
                Actions.Rest();
            });
            
            UnityEngine.Debug.Log($"REST");
        }
        else
        {
            Console.WriteLine("Invalid move command.");
        }
    }

    private void HandleFreeze() 
    {
        mainThreadActions.Enqueue(() => {
            Actions.Freeze();
        });
    }

    private void HandleBackpack() 
    { 
        mainThreadActions.Enqueue(() => {
            Actions.IncreaseBacpackStorage();
        });
    }

    private void HandleDaze() 
    { 
        mainThreadActions.Enqueue(() => {
            Actions.Daze();
        });
    }

    private void InvalidTurnHandling() 
    { 

    }

    private void TimeoutHandling() 
    { 

    }


}
