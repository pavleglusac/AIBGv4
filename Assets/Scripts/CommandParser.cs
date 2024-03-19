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

    string attackPattern = @"attack (-?\d+) (-?\d+)$";

    string putRefinement = @"refinement-put (-?\d+) (-?\d+) cheap (-?\d+) expensive (-?\d+)";

    string takeRefinement = @"refinement-take (-?\d+) (-?\d+) cheap (-?\d+) expensive (-?\d+)";

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
        UnityEngine.Debug.Log($"PARSING: {command}");
        if (command == null)
        {
            return;
        }
        if (command.StartsWith("//")) 
        {
            // UnityEngine.Debug.Log($"{command}");
            return;
        }

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
            HandleResting(command);
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
        else if (Regex.IsMatch(command, putRefinement))
        {
            HandlePutRefinement(command);
        }
        else if (Regex.IsMatch(command, takeRefinement))
        {
            HandleTakeRefinement(command);
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
            int x = int.Parse(match.Groups[1].Value);
            int z = int.Parse(match.Groups[2].Value);

            mainThreadActions.Enqueue(() => {
                GameObject commandObject = new GameObject("MoveCommandObject");
                MoveCommand moveCommandInstance = commandObject.AddComponent<MoveCommand>();
                moveCommandInstance.Initialize(x, z);
                Game.Instance.CommandManager.AddCommand(moveCommandInstance);
            });
            
            UnityEngine.Debug.Log($"MOVE {x} {z}");
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
                Actions.BuildHouse(x, z);
            });
            
            UnityEngine.Debug.Log($"MINE {x} {z}");
        }
        else
        {
            Console.WriteLine("Invalid move command.");
        }
    }

    private void HandlePutRefinement(string command)
    {
        var match = Regex.Match(command, buildPattern);
        if (match.Success)
        {
            int x = int.Parse(match.Groups[1].Value);
            int z = int.Parse(match.Groups[2].Value);
            int cheap = int.Parse(match.Groups[3].Value);
            int expensive = int.Parse(match.Groups[4].Value);
            mainThreadActions.Enqueue(() => {
                Actions.PutRefinement(x, z, cheap, expensive);
            });
            
            UnityEngine.Debug.Log($"PUT REFINEMENT {x} {z} {cheap} {expensive}");
        }
        else
        {
            Console.WriteLine("Invalid move command.");
        }
    }

    private void HandleTakeRefinement(string command)
    {
        var match = Regex.Match(command, buildPattern);
        if (match.Success)
        {

        }
        else
        {
            
        }
    }

    private void HandleConversionsAtBase(string command) 
    {
        var match = Regex.Match(command, buildPattern);
        if (match.Success)
        {

        }
        else
        {
            
        }
    }

    private void HandleResting(string command) 
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
