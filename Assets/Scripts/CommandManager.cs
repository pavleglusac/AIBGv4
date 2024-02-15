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

        if (_currentCommand == null || _currentCommand.IsDone()) return;

        if (_currentCommand.CanExecute())
        {
            _currentCommand.Execute();
            Game.Instance.SwitchPlayersAndDecreaseStats();
        }
        else switch (_currentCommand)
        {
            case IEnergySpendingCommand:
                Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
                if (Game.Instance.GameOver)
                {
                    Game.Instance.Winner = Game.Instance.FirstPlayerTurn ? Game.Instance.Player1.Name : Game.Instance.Player2.Name;
                    Debug.Log("Game over");
                    Debug.Log("Winner is: " + Game.Instance.Winner);
                    Game.EndGame();
                }
                else
                {
                    Game.Instance.SwitchPlayersAndDecreaseStats();
                }
                break;
            case ICoinSpendingCommand:
                Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
                Game.Instance.SwitchPlayersAndDecreaseStats();
                break;
            default:
                Game.Instance.GetCurrentPlayer().InvalidMoveTakeEnergy();
                Game.Instance.SwitchPlayersAndDecreaseStats();
                break;
        }
        _currentCommand = null;
        _index++;
    }

}
