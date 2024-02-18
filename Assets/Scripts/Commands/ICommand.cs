using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// create abstract class for command
public interface ICommand
{

    public Player Player { get; set; }
    public void Execute();
    public bool IsDone();
    public bool CanExecute();
    // TODO: Ovo ce nam biti korisno za replay opciju
    // public void Undo();
}