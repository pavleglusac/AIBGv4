using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }
    private List<ICommand> _commands = new List<ICommand>();
    private ICommand _currentCommand;
    private int _index = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
            Game.Instance.SwitchPlayersAndDecreaseStats();
        }
        else { 
            Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
            if (Game.Instance.TurnCount > int.Parse(PlayerPrefs.GetString("max_number_of_turns")))
            {
                Game.Instance.GameOver = true;
                Game.Instance.Winner = Game.Instance.Player1.XP > Game.Instance.Player2.XP ? Game.Instance.Player1.Name : Game.Instance.Player2.Name;
            }
            if (Game.Instance.GetCurrentPlayer().Energy <= 0)
            {
                Game.Instance.GameOver = true;
                Game.Instance.Winner = Game.Instance.FirstPlayerTurn ? Game.Instance.Player2.Name : Game.Instance.Player1.Name;
            }
            if (Game.Instance.GameOver)
            {
                Game.EndGame();
            }
            else
            {
                Game.Instance.SwitchPlayersAndDecreaseStats();
            }
        }
        //If you get this message that means that you have not put correct display message for your behaviour
        Game.Instance.DisplayMessage = "UNDEFINED MESSAGE!";

        _currentCommand = null;
        _index++;
    }

}
