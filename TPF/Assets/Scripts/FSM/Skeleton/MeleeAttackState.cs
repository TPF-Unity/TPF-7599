using System.Collections;
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
        agent = fsm.GetComponent<NavMeshAgent>();
        animationController = fsm.GetComponent<NPCAnimationController>();
        player = GameObject.Find("Player").transform;
        transform = fsm.transform;
        timeSinceLastAttack = 0f;
        isAttacking = false;
    }

    protected override void ExecuteState(FSM fsm)
    {
        agent.SetDestination(fsm.transform.position);

        fsm.transform.LookAt(player);

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
