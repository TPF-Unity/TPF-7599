using System;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    private State _currentState;
    public State CurrentState
    {
        get { return _currentState; }
        set
        {
            Debug.Log("set state");
            Debug.Log(value);
            if (_currentState != value)
            {
                if (_currentState != null)
                    _currentState.ExitState(this);

                _currentState = value;

                if (_currentState != null)
                    _currentState.EnterState(this);
            }
        }
    }

    private void Update()
    {
        if (_currentState != null)
            _currentState.Execute(this);
    }
}
