using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkeletonController : EnemyNPCController
{

    bool alreadyAttacked;

    private Vector3 attackSpawnPoint;
    [SerializeField] private NPCStats stats;

    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animationController = new SkeletonAnimationController();
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
            attackSpawnPoint = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z + 0.5f);
            GameObject slash = Instantiate(projectile, attackSpawnPoint, transform.rotation);
            slash.layer = LayerMask.NameToLayer("EnemiesProjectiles");
            Slash slashAsset = slash.GetComponent<Slash>();
            slashAsset.Execute();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), 1f / stats.AttackSpeed);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
