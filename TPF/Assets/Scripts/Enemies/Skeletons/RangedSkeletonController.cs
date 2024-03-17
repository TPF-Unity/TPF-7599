using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeletonController : SkeletonController
{
    protected override void InitializeFSM()
    {
        PlayerInRangeCondition playerInSight = PlayerInRangeCondition.Create(stats.SightRange, whatIsPlayer);
        PlayerInRangeCondition lostPlayer = PlayerInRangeCondition.Create(stats.SightRange, whatIsPlayer, false);
        PlayerInRangeCondition playerInAttackRange = PlayerInRangeCondition.Create(stats.AttackRange, whatIsPlayer);
        PlayerInRangeCondition playerUnreachable = PlayerInRangeCondition.Create(stats.AttackRange, whatIsPlayer, false);
        PatrolState patrolState = ScriptableObject.CreateInstance<PatrolState>();
        ChaseState chaseState = ScriptableObject.CreateInstance<ChaseState>();
        RangedAttackState attackState = RangedAttackState.Create(projectile, stats.AttackSpeed, stats.Damage);
        Transition patrolToChaseTransition = Transition.Create(chaseState, playerInSight);
        Transition chaseToPatrolTransition = Transition.Create(patrolState, lostPlayer);
        Transition chaseToAttackTransition = Transition.Create(attackState, playerInAttackRange);
        Transition attackToChaseTransition = Transition.Create(chaseState, playerUnreachable);
        patrolState.AddTransition(patrolToChaseTransition);
        chaseState.AddTransition(chaseToPatrolTransition);
        chaseState.AddTransition(chaseToAttackTransition);
        attackState.AddTransition(attackToChaseTransition);
        FSM fsm = GetComponent<FSM>();
        fsm.CurrentState = patrolState;
    }
}
