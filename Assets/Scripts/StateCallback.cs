using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCallback : StateMachineBehaviour
{
    private static int entered = 0;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        entered++;
        if(entered % 3 == 0)
        {
            Game.Instance.InvokeScript(Game.Instance.FirstPlayerTurn);
            Game.Instance.ArePlayersLanded = true;
        }
        
    }
}
