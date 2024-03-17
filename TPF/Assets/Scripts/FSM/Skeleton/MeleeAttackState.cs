using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class AttackState : State
{
    protected Vector3 attackSpawnPoint;
    protected bool alreadyAttacked;
    protected float attackSpeed;
    protected float damage;
    protected GameObject projectile;

    protected NavMeshAgent agent;
    protected NPCAnimationController animationController;
    protected Transform player;
    protected Transform transform;
    protected float timeSinceLastAttack;

    public static AttackState Create(GameObject projectile, float attackSpeed, float damage)
    {
        AttackState state = CreateInstance<AttackState>();
        state.projectile = projectile;
        state.attackSpeed = attackSpeed;
        state.damage = damage;
        return state;
    }

    public override void EnterState(FSM fsm)
    {
        agent = fsm.GetComponent<NavMeshAgent>();
        animationController = fsm.GetComponent<NPCAnimationController>();
        player = GameObject.Find("Player").transform;
        transform = fsm.transform;
        alreadyAttacked = false;
        timeSinceLastAttack = 0f;
    }

    protected override void ExecuteState(FSM fsm)
    {
        agent.SetDestination(fsm.transform.position);

        fsm.transform.LookAt(player);

        if (!alreadyAttacked || timeSinceLastAttack >= 1f / attackSpeed)
        {
            animationController.PlayAnimation(AnimationType.Attack);
            ExecuteAttack();
            ResetAttack();
            timeSinceLastAttack = 0f;
        }

        timeSinceLastAttack += Time.deltaTime;
    }

    protected virtual void ExecuteAttack()
    {
        attackSpawnPoint = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z + 0.5f);
        GameObject slash = Instantiate(projectile, attackSpawnPoint, transform.rotation);
        slash.layer = LayerMask.NameToLayer("EnemiesProjectiles");
        slash.GetComponent<Slash>().Damage = damage;
        Slash slashAsset = slash.GetComponent<Slash>();
        slashAsset.Execute();
    }

    private void ResetAttack()
    {
        alreadyAttacked = true;
    }
}