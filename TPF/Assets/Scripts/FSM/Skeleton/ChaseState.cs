using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    private Transform player;
    private NavMeshAgent agent;

    public override void EnterState(FSM fsm)
    {
        player = GameObject.Find("Player").transform;
        agent = fsm.GetComponent<NavMeshAgent>();
    }

    protected override void ExecuteState(FSM fsm)
    {
        agent.SetDestination(player.position);
    }
}
