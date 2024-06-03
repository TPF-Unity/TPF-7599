using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Misc;

public class RangedAttackState : AttackState
{
    public static new RangedAttackState Create(GameObject projectile, float attackSpeed, float damage, Transform attackSpawnPoint)
    {
        RangedAttackState state = CreateInstance<RangedAttackState>();
        state.projectile = projectile;
        state.attackSpeed = attackSpeed;
        state.damage = damage;
        state.attackSpawnPoint = attackSpawnPoint;
        return state;
    }

    override protected void ExecuteAttack()
    {
        GameObject arrow = Instantiate(projectile, attackSpawnPoint.position, transform.rotation);
        arrow.layer = LayerMask.NameToLayer(Layer.EnemyProjectiles.ToString());
        arrow.GetComponent<Arrow>().Damage = damage;
        Arrow arrowAsset = arrow.GetComponent<Arrow>();
        arrowAsset.Shoot(player.transform.position);
        isAttacking = false;
    }
}
