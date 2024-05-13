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
    string conversionsPattern = @"^conv (\d+) diamond (\d+) mineral to coins, (\d+) diamond (\d+) mineral to energy, (\d+) diamond (\d+) mineral to xp$";
    string restPattern = @"^rest";
    string shopPattern = @"^shop (freeze|backpack|daze)$";

    string attackPattern = @"attack (-?\d+) (-?\d+)$";

    string putRefinement = @"refinement-put (-?\d+) (-?\d+) mineral (-?\d+) diamond (-?\d+)";

    string takeRefinement = @"refinement-take (-?\d+) (-?\d+) mineral (-?\d+) diamond (-?\d+)";

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
    

    public void ParseCommand(string command, string playerName) 
    {
        UnityEngine.Debug.Log($"Player {playerName} - PARSING: {command}");
        if (command == null)
        {
            UnityEngine.Debug.Log("Invalid input, command not recognised!");
            InvalidTurnHandling(command);
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
        else if (Regex.IsMatch(command, attackPattern))
        {
            HandleAttack(command);
        }
        else
        {
            UnityEngine.Debug.Log("Invalid input, command not recognised!");
            InvalidTurnHandling(command);
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
                UnityEngine.Debug.Log($"ACTION: MOVE {x} {z}");
                GameObject commandObject = new GameObject("MoveCommandObject");
                MoveCommand moveCommandInstance = commandObject.AddComponent<MoveCommand>();
                moveCommandInstance.Initialize(x, z);
                Game.Instance.CommandManager.AddCommand(moveCommandInstance);
            });
            
        }
        else
        {
            UnityEngine.Debug.Log("Invalid move command.");
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
                UnityEngine.Debug.Log($"ACTION: MINE {x} {z}");
                Actions.Mine(x, z);
            });
            
        }
        else
        {
            UnityEngine.Debug.Log("Invalid move command.");
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
                UnityEngine.Debug.Log($"ACTION: BUILD {x} {z}");
                Actions.BuildHouse(x, z);
            });
            
        }
        else
        {
            UnityEngine.Debug.Log("Invalid move command.");
        }
    }

    private void HandleAttack(string command)
    {
        var match = Regex.Match(command, buildPattern);
        if (match.Success)
        {
            int x = int.Parse(match.Groups[1].Value);
            int z = int.Parse(match.Groups[2].Value);

            mainThreadActions.Enqueue(() => {
                UnityEngine.Debug.Log($"ACTION: ATTACK {x} {z}");
                Actions.AttackHouse(x, z);
            });

        }
        else
        {
            UnityEngine.Debug.Log("Invalid move command.");
        }
    }

    private void HandlePutRefinement(string command)
    {
        var match = Regex.Match(command, putRefinement);
        if (match.Success)
        {
            int x = int.Parse(match.Groups[1].Value);
            int z = int.Parse(match.Groups[2].Value);
            int cheap = int.Parse(match.Groups[3].Value);
            int expensive = int.Parse(match.Groups[4].Value);
            mainThreadActions.Enqueue(() => {
                UnityEngine.Debug.Log($"ACTION: PUT REFINEMENT {x} {z} {cheap} {expensive}");
                Actions.PutRefinement(x, z, cheap, expensive);
            });
            
        }
        else
        {
            UnityEngine.Debug.Log("Invalid move command.");
        }
    }

    private void HandleTakeRefinement(string command)
    {
        var match = Regex.Match(command, takeRefinement);
        if (match.Success)
        {
            int x = int.Parse(match.Groups[1].Value);
            int z = int.Parse(match.Groups[2].Value);
            int cheap = int.Parse(match.Groups[3].Value);
            int expensive = int.Parse(match.Groups[4].Value);
            mainThreadActions.Enqueue(() => {
                UnityEngine.Debug.Log($"ACTION: TAKE REFINEMENT {x} {z} {cheap} {expensive}");
                Actions.TakeRefinement(x, z, cheap, expensive);
            });

        }
        else
        {
            UnityEngine.Debug.Log("Invalid move command.");
        }
    }

    //string conversionsPattern = @"^conv (\d+) diamond (\d+) mineral to coins, (\d+) diamond (\d+) mineral to energy, (\d+) diamond (\d+) mineral to xp$";

    private void HandleConversionsAtBase(string command) 
    {
        var match = Regex.Match(command, conversionsPattern);
        if (match.Success)
        {
            int expCoins = int.Parse(match.Groups[1].Value);
            int cheapCoins = int.Parse(match.Groups[2].Value);
            int cheapEnergy = int.Parse(match.Groups[4].Value);
            int expEnergy = int.Parse(match.Groups[3].Value);
            int expXP = int.Parse(match.Groups[5].Value);
            int cheapXP = int.Parse(match.Groups[6].Value);
            mainThreadActions.Enqueue(() => {
                UnityEngine.Debug.Log($"ACTION: BASE CONVERSIONS {cheapCoins} {expCoins} {cheapEnergy} {expEnergy} {cheapXP} {expXP}");
                Actions.BaseConversions(cheapXP, expXP, cheapCoins, expCoins, cheapEnergy, expEnergy);
            });

        }
        else
        {
            UnityEngine.Debug.Log("Invalid move command.");
        }
    }

    private void HandleResting(string command) 
    {
        var match = Regex.Match(command, restPattern);
        if (match.Success)
        {

            mainThreadActions.Enqueue(() => {
                UnityEngine.Debug.Log($"ACTION: REST");
                Actions.Rest();
            });
            
        }
        else
        {
            UnityEngine.Debug.Log("Invalid move command.");
        }
    }

    private void HandleFreeze() 
    {
        mainThreadActions.Enqueue(() => {
            UnityEngine.Debug.Log($"ACTION: FREEZE");
            Actions.Freeze();
        });
    }

    private void HandleBackpack() 
    { 
        mainThreadActions.Enqueue(() => {
            UnityEngine.Debug.Log($"ACTION: BACKPACK");
            Actions.IncreaseBacpackStorage();
        });
    }

    private void HandleDaze() 
    { 
        mainThreadActions.Enqueue(() => {
            UnityEngine.Debug.Log($"ACTION: DAZE");
            Actions.Daze();
        });
    }

    private void InvalidTurnHandling(string command) 
    {
        if(command == null) {
            command = "No command.";
        }
        mainThreadActions.Enqueue(() => {
            Actions.InvalidAction();
            Game.Instance.DisplayMessage = $"Fail to parse: {command.Trim()}";
        });   
    }


    public void FinishGame(string message) 
    {
        mainThreadActions.Enqueue(() => {
            string winnerName = Game.Instance.FirstPlayerTurn ? Game.Instance.Player2.Name : Game.Instance.Player1.Name;
            string loserName = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1.Name : Game.Instance.Player2.Name;
            Game.Instance.Winner = winnerName;
            message = $"Turn for {loserName}\n{message}";
            Game.EndGame(message);
        });
    }


}
