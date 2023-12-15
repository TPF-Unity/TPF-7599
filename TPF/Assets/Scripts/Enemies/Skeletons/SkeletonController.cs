using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkeletonController : EnemyNPCController
{

    bool alreadyAttacked;
    protected Vector3 attackSpawnPoint;
    [SerializeField] protected NPCStats stats;

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
        Animator animator = GetComponent<Animator>();
        animationController = AnimatorController switch
        {
            AnimatorControllerType.WizardSkeleton => new WizardSkeletonAnimationController(),
            AnimatorControllerType.MeleeSkeleton => new SkeletonAnimationController(),
            AnimatorControllerType.ArcherSkeleton => new ArcherSkeletonAnimationController(),
            _ => throw new ArgumentOutOfRangeException(),
        };
        animationController.Initialize(animator);
        stats = GetComponent<Unit>().stats;
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
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

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
