using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArcherSkeletonController : EnemyNPCController
{
    [SerializeField] private NPCStats stats;
    bool alreadyAttacked;
    float lastAttackTime;

    private Vector3 attackSpawnPoint;


    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animationController = new ArcherSkeletonAnimationController();
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
            Invoke(nameof(Shoot), 1f / stats.AttackSpeed); // TODO: Synchronize with animation
            Invoke(nameof(ResetAttack), 2f / stats.AttackSpeed);
            alreadyAttacked = true;
        }
    }

    private void Shoot()
    {
        attackSpawnPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.5f);
        GameObject arrow = Instantiate(projectile, attackSpawnPoint, transform.rotation);
        arrow.layer = LayerMask.NameToLayer("EnemiesProjectiles");
        arrow.GetComponent<Arrow>().Damage = stats.Damage;
        Arrow arrowAsset = arrow.GetComponent<Arrow>();
        arrowAsset.Shoot(player.transform.position);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
