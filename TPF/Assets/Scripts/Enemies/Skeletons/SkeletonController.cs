using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : EnemyNPCController
{
    public enum AnimatorControllerType
    {
        WizardSkeleton,
        MeleeSkeleton,
        ArcherSkeleton,
    }

    public enum AttackType
    {
        Melee,
        Ranged,
    }

    public AnimatorControllerType AnimatorController;

    private void Start()
    {
        animationController = AnimatorController switch
        {
            AnimatorControllerType.WizardSkeleton => gameObject.AddComponent<WizardSkeletonAnimationController>(),
            AnimatorControllerType.MeleeSkeleton => gameObject.AddComponent<SkeletonAnimationController>(),
            AnimatorControllerType.ArcherSkeleton => gameObject.AddComponent<ArcherSkeletonAnimationController>(),
            _ => throw new ArgumentOutOfRangeException(),
        };
        animationController.Initialize(animator);
        stats = GetComponent<Unit>().stats;
        agent.speed = stats.MovementSpeed;
        InitializeFSM();
    }

    protected virtual void InitializeFSM()
    {
        PlayerInRangeCondition playerInSight = PlayerInRangeCondition.Create(stats.SightRange, whatIsPlayer);
        PlayerInRangeCondition lostPlayer = PlayerInRangeCondition.Create(stats.SightRange, whatIsPlayer, false);
        PlayerInRangeCondition playerInAttackRange = PlayerInRangeCondition.Create(stats.AttackRange, whatIsPlayer);
        PlayerInRangeCondition playerUnreachable = PlayerInRangeCondition.Create(stats.AttackRange, whatIsPlayer, false);
        PatrolState patrolState = PatrolState.Create();
        ChaseState chaseState = ChaseState.Create();
        AttackState attackState = AttackState.Create(projectile, stats.AttackSpeed, stats.Damage);
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
