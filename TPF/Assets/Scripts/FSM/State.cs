using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : ScriptableObject
{
    public List<Transition> Transitions = new List<Transition>();

    public void Execute(FSM fsm)
    {
        ExecuteState(fsm);

        foreach (var transition in Transitions)
        {
            transition.ExecuteTransition(fsm);
        }
    }


    /// <summary>
    ///  Should be called by Transition when the FSM enters this state
    /// </summary>
    public virtual void EnterState(FSM fsm) { }

    /// <summary>
    /// Should be called by Transition when the FSM exits this state
    /// </summary>
    public virtual void ExitState(FSM fsm) { }

    protected virtual void ExecuteState(FSM fsm) { }
}
