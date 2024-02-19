using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    private Transform player;
    private NavMeshAgent agent;
    private NPCAnimationController animationController;

    public override void EnterState(FSM fsm)
    {
        player = GameObject.Find("Player").transform;
        agent = fsm.GetComponent<NavMeshAgent>();
        animationController = fsm.GetComponent<NPCAnimationController>();
    }

    protected override void ExecuteState(FSM fsm)
    {
        animationController.PlayAnimation(AnimationType.Walk);
        agent.SetDestination(player.position);
    }
}
