using System.Collections.Generic;
using UnityEngine;


public class CommandManager : MonoBehaviour
{
    private List<ICommand> _commands = new List<ICommand>();
    private ICommand _currentCommand;
    private ICommand _lastCommand;
    private int _index = 0;

    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
    }

    private void LateUpdate()
    {
        if (Game.Instance.GameOver)
        {
            return;
        }

        if (_lastCommand != null && _lastCommand.IsDone())
        {
            _lastCommand = null;
            Game.Instance.SwitchPlayersAndDecreaseStats();
            Game.Instance.DisplayMessage = "UNDEFINED MESSAGE!";
            return;
        }
        else if (_lastCommand != null && !_lastCommand.IsDone())
        {
            return;
        }

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
            _currentCommand.isDone = true;
            Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
            UnityEngine.Debug.Log($"Can't execute ACTION. Reason: {Game.Instance.DisplayMessage}");
        }
        string gameOverMessage = "";

        if (Game.Instance.GetCurrentPlayer().Energy <= 0)
        {
            Game.Instance.GameOver = true;
            Game.Instance.Winner = Game.Instance.FirstPlayerTurn ? Game.Instance.Player2.Name : Game.Instance.Player1.Name;
            gameOverMessage = Game.Instance.GetCurrentPlayer().Name + " has no energy left!";
        }
        else
        {
            if (Game.Instance.TurnCount + 1 >= int.Parse(PlayerPrefs.GetString("max_number_of_turns")))
            {
                Player winner = GetWinner(ref gameOverMessage);
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
        }

        if (Game.Instance.GameOver)
        {
            Game.EndGame(gameOverMessage);
        }

        //If you get this message that means that you have not put correct display message for your behaviour
        
        _lastCommand = _currentCommand;
        _currentCommand = null;
        _index++;
    }

    private Player GetWinner(ref string gameOverMessage)
    {
        if (Game.Instance.Player1.XP > Game.Instance.Player2.XP)
        {
            gameOverMessage = Game.Instance.Player1.Name + " had more XP at the end of the game";
            return Game.Instance.Player1;
        }
        else if (Game.Instance.Player1.XP < Game.Instance.Player2.XP)
        {
            gameOverMessage = Game.Instance.Player2.Name + " had more XP at the end of the game";
            return Game.Instance.Player2;
        }

        if (Game.Instance.Player1.Coins > Game.Instance.Player2.Coins)
        {
            gameOverMessage = Game.Instance.Player1.Name + " had more coins at the end of the game";
            return Game.Instance.Player1;
        }
        else if (Game.Instance.Player1.Coins < Game.Instance.Player2.Coins)
        {
            gameOverMessage = Game.Instance.Player2.Name + " had more coins at the end of the game";
            return Game.Instance.Player2;
        }

        if (Game.Instance.Player1.Energy > Game.Instance.Player2.Energy)
        {
            gameOverMessage = Game.Instance.Player1.Name + " had more energy at the end of the game";
            return Game.Instance.Player1;
        }
        else if (Game.Instance.Player1.Energy < Game.Instance.Player2.Energy)
        {
            gameOverMessage = Game.Instance.Player2.Name + " had more energy at the end of the game";
            return Game.Instance.Player2;
        }

        if (Game.Instance.Player1.Bag.GetWeight() > Game.Instance.Player2.Bag.GetWeight())
        {
            gameOverMessage = Game.Instance.Player1.Name + "'s bag was heavier at the end of the game";
            return Game.Instance.Player1;
        }
        else if (Game.Instance.Player1.Bag.GetWeight() < Game.Instance.Player2.Bag.GetWeight())
        {
            gameOverMessage = Game.Instance.Player2.Name + "'s bag was heavier at the end of the game";
            return Game.Instance.Player2;
        }

        if (Game.Instance.Board.CountPlayersHouses(true) > Game.Instance.Board.CountPlayersHouses(false))
        {
            gameOverMessage = Game.Instance.Player1.Name + " had more refinement facilities at the end of the game";
            return Game.Instance.Player1;
        }
        else if (Game.Instance.Board.CountPlayersHouses(true) < Game.Instance.Board.CountPlayersHouses(false))
        {
            gameOverMessage = Game.Instance.Player2.Name + " had more  refinement facilities at the end of the game";
            return Game.Instance.Player2;
        }

        gameOverMessage = "Players were matched in skill completely!";
        return null;
    }
}
