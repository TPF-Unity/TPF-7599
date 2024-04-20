using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : ScriptableObject
{
    protected List<Transition> transitions = new List<Transition>();

    public void Execute(FSM fsm)
    {
        ExecuteState(fsm);

        foreach (var transition in transitions)
        {
            transition.ExecuteTransition(fsm);
        }
    }

    public void AddTransition(Transition transition)
    {
        transitions.Add(transition);
    }

    public virtual void EnterState(FSM fsm) { }

    public virtual void ExitState(FSM fsm) { }

    protected virtual void ExecuteState(FSM fsm) { }
}
