using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WizardSkeletonController : EnemyNPCController
{

    bool alreadyAttacked;

    private Vector3 attackSpawnPoint;


    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animationController = new WizardSkeletonAnimationController();
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
            attackSpawnPoint = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z + 0.5f);
            GameObject slash = Instantiate(projectile, attackSpawnPoint, transform.rotation);
            slash.layer = LayerMask.NameToLayer("EnemiesProjectiles");
            Slash slashAsset = slash.GetComponent<Slash>();
            slashAsset.Execute();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
