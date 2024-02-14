using System;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField] private State _initialState;

    private void Awake()
    {
        CurrentState = _initialState;
        CurrentState.EnterState(this);
    }

    public State CurrentState { get; set; }

    private void Update()
    {
        CurrentState.Execute(this);
    }
}
