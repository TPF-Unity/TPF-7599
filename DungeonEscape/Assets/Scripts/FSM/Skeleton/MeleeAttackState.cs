using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Misc;

public class AttackState : State
{
    protected Transform attackSpawnPoint;
    protected float attackSpeed;
    protected float damage;
    protected GameObject projectile;

    protected NavMeshAgent agent;
    protected NPCAnimationController animationController;
    protected Transform player;
    protected Transform transform;
    protected float timeSinceLastAttack;

    protected bool isAttacking;

    public static AttackState Create(GameObject projectile, float attackSpeed, float damage, Transform attackSpawnPoint)
    {
        AttackState state = CreateInstance<AttackState>();
        state.projectile = projectile;
        state.attackSpeed = attackSpeed;
        state.damage = damage;
        state.attackSpawnPoint = attackSpawnPoint;
        return state;
    }

    public override void EnterState(FSM fsm)
    {
        transform = fsm.transform;
        agent = fsm.GetComponent<NavMeshAgent>();
        animationController = fsm.GetComponent<NPCAnimationController>();
        var players = GameObject.FindGameObjectsWithTag("Player");
        player = players.OrderBy(targetPlayer => Vector3.Distance(targetPlayer.transform.position, transform.position))
            .First()?.transform;
        timeSinceLastAttack = 0f;
        isAttacking = false;
    }

    private void lookAtHorizontal(Transform targetTransform)
    {
        if (transform == null || targetTransform == null)
        {
            return;
        }

        Vector3 lookDirection = targetTransform.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 300);
        }
    }

    protected override void ExecuteState(FSM fsm)
    {
        agent.SetDestination(fsm.transform.position);

        lookAtHorizontal(player);

        if (!isAttacking)
        {
            isAttacking = true;
            animationController.PlayAnimation(AnimationType.Attack);
            GameManager.instance.StartCoroutine(WaitForAttack());
            timeSinceLastAttack = 0f;
        }

        timeSinceLastAttack += Time.deltaTime;
    }

    protected virtual void ExecuteAttack()
    {
        GameObject slash = Instantiate(projectile, attackSpawnPoint.position, transform.rotation);
        slash.layer = LayerMask.NameToLayer(Layer.EnemyProjectiles.ToString());
        slash.GetComponent<Slash>().Damage = damage;
        Slash slashAsset = slash.GetComponent<Slash>();
        slashAsset.Execute();
        isAttacking = false;
    }

    private IEnumerator WaitForAttack()
    {
        while (animationController.CanAttack() == false)
        {
            yield return null;
        }

        animationController.SetAttack(false);
        ExecuteAttack();
    }
}