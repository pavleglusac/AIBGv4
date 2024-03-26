using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CommandManager : MonoBehaviour
{
    private List<ICommand> _commands = new List<ICommand>();
    private ICommand _currentCommand;
    private int _index = 0;

    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
    }

    private void LateUpdate()
    {
        if (_index >= _commands.Count)
        {
            return;
        }
        if (_currentCommand == null && _commands.Count > 0)
        {
            _currentCommand = _commands[_index];
            // _currentCommand.Execute();
            //Debug.Log("Executing command: " + _currentCommand.ToString());
        }
        else if (_currentCommand != null && _currentCommand.IsDone())
        {
            _currentCommand = null;
            _index++;
        }

        if (_currentCommand == null || _currentCommand.IsDone()) return;

        if (_currentCommand.CanExecute())
        {
            _currentCommand.Execute();

        }
        else
        {
            Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
        }

        if (Game.Instance.GetCurrentPlayer().Energy <= 0)
        {
            Game.Instance.GameOver = true;
            Game.Instance.Winner = Game.Instance.FirstPlayerTurn ? Game.Instance.Player2.Name : Game.Instance.Player1.Name;
        }


        if (Game.Instance.TurnCount + 1 >= int.Parse(PlayerPrefs.GetString("max_number_of_turns")))
        {
            Player winner = GetWinner();
            Game.Instance.GameOver = true;

            if (winner != null)
            {
                Game.Instance.Winner = winner.Name;
            }
            else
            {
                Game.Instance.Winner = "NO WINNER - PLAY AGAIN";
            }
        }


        if (Game.Instance.GameOver)
        {
            Game.EndGame();
        }
        else
        {
            Game.Instance.SwitchPlayersAndDecreaseStats();
        }

        //If you get this message that means that you have not put correct display message for your behaviour
        Game.Instance.DisplayMessage = "UNDEFINED MESSAGE!";

        _currentCommand = null;
        _index++;
    }

    private Player GetWinner()
    {
        if (Game.Instance.Player1.XP > Game.Instance.Player2.XP)
            return Game.Instance.Player1;

        if (Game.Instance.Player1.XP < Game.Instance.Player2.XP)
            return Game.Instance.Player2;

        if (Game.Instance.Player1.Coins > Game.Instance.Player2.Coins)
            return Game.Instance.Player1;

        if (Game.Instance.Player1.Coins < Game.Instance.Player2.Coins)
            return Game.Instance.Player2;

        if (Game.Instance.Player1.Energy > Game.Instance.Player2.Energy)
            return Game.Instance.Player1;

        if (Game.Instance.Player1.Energy < Game.Instance.Player2.Energy)
            return Game.Instance.Player1;

        if (Game.Instance.Player1.Bag.GetWeight() > Game.Instance.Player2.Bag.GetWeight())
            return Game.Instance.Player1;

        if (Game.Instance.Player1.Bag.GetWeight() < Game.Instance.Player2.Bag.GetWeight())
            return Game.Instance.Player1;

        if (Game.Instance.Board.CountPlayersHouses(true) > Game.Instance.Board.CountPlayersHouses(false))
            return Game.Instance.Player1;

        if (Game.Instance.Board.CountPlayersHouses(true) < Game.Instance.Board.CountPlayersHouses(false))
            return Game.Instance.Player1;

        return null;
    }
}
