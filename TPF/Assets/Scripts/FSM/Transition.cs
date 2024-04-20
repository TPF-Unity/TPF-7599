using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : ScriptableObject
{
    private State newState;
    private Condition condition;

    public static Transition Create(State newState, Condition condition)
    {
        Transition transition = ScriptableObject.CreateInstance<Transition>();
        transition.newState = newState;
        transition.condition = condition;
        return transition;
    }

    public void ExecuteTransition(FSM fsm)
    {
        if (condition.ConditionIsMet(fsm))
        {
            fsm.CurrentState = newState;
        }
    }
}
