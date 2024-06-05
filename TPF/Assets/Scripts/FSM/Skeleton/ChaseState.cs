using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    private Transform player;
    private NavMeshAgent agent;
    private NPCAnimationController animationController;

    public static ChaseState Create()
    {
        ChaseState state = CreateInstance<ChaseState>();
        return state;
    }

    public override void EnterState(FSM fsm)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        player = players.OrderBy(targetPlayer => Vector3.Distance(targetPlayer.transform.position, fsm.transform.position))
            .First()?.transform;
        agent = fsm.GetComponent<NavMeshAgent>();
        animationController = fsm.GetComponent<NPCAnimationController>();
    }

    protected override void ExecuteState(FSM fsm)
    {
        animationController.PlayAnimation(AnimationType.Walk);
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
        
    }
}
