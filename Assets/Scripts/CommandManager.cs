using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
        if (_currentCommand != null && !_currentCommand.IsDone())
        {
            if(_currentCommand.CanExecute())
            {
                Debug.Log("Executing command: " + _currentCommand.ToString());
                _currentCommand.Execute();
            }
            else if(_currentCommand is IEnergySpendingCommand)
            {
                Game.Instance.Winner = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1.Name : Game.Instance.Player2.Name;
                Game.Instance.GameOver = true;
                Debug.Log("Game over");
                Debug.Log("Winner is: " + Game.Instance.Winner);   
                Game.EndGame();
                
            }
            _currentCommand = null;
            _index++;
        }
    }

}
