using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transition")]
public sealed class Transition : ScriptableObject
{
    public State NewState;
    public Condition Condition;

    public void ExecuteTransition(FSM fsm)
    {
        if (Condition.ConditionIsMet(fsm))
        {
            fsm.CurrentState.ExitState(fsm);
            fsm.CurrentState = NewState;
            fsm.CurrentState.EnterState(fsm);
        }
    }
}
