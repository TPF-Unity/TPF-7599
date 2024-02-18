using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkeletonController : EnemyNPCController
{
    protected Vector3 attackSpawnPoint;

    public enum AnimatorControllerType
    {
        WizardSkeleton,
        MeleeSkeleton,
        ArcherSkeleton,
    }

    public AnimatorControllerType AnimatorController;

    public enum AttackType
    {
        Melee,
        Ranged,
    }

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

        PlayerInRangeCondition playerInSight = PlayerInRangeCondition.Create(stats.SightRange, whatIsPlayer);
        PlayerInRangeCondition lostPlayer = PlayerInRangeCondition.Create(stats.SightRange, whatIsPlayer, false);
        PatrolState patrolState = ScriptableObject.CreateInstance<PatrolState>();
        ChaseState chaseState = ScriptableObject.CreateInstance<ChaseState>();
        Transition patrolToChaseTransition = Transition.Create(chaseState, playerInSight);
        Transition chaseToPatrolTransition = Transition.Create(patrolState, lostPlayer);
        patrolState.AddTransition(patrolToChaseTransition);
        chaseState.AddTransition(chaseToPatrolTransition);
        FSM fsm = GetComponent<FSM>();
        fsm.CurrentState = patrolState;
    }

    protected override void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animationController.PlayAnimation(AnimationType.Attack);
            Invoke(nameof(ExecuteAttack), 1f / stats.AttackSpeed); // TODO: Synchronize with animation
            Invoke(nameof(ResetAttack), 1f / stats.AttackSpeed);
            alreadyAttacked = true;
        }
    }

    protected abstract void ExecuteAttack();
}
