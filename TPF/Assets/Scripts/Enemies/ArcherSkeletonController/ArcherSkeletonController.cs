using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArcherSkeletonController : EnemyNPCController
{

    bool alreadyAttacked;
    float lastAttackTime;

    private Vector3 attackSpawnPoint;


    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animationController = new ArcherSkeletonAnimationController();
        animationController.Initialize(animator);
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
            Invoke(nameof(Shoot), timeBetweenAttacks / 2); // TODO: Synchronize with animation
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            alreadyAttacked = true;
        }
    }

    private void Shoot()
    {
        attackSpawnPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.5f);
        GameObject arrow = Instantiate(projectile, attackSpawnPoint, transform.rotation);
        arrow.layer = LayerMask.NameToLayer("EnemiesProjectiles");
        Arrow arrowAsset = arrow.GetComponent<Arrow>();
        arrowAsset.Shoot(player.transform.position);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
