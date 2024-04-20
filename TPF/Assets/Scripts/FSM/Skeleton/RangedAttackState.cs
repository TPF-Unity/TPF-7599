using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Misc;

public class RangedAttackState : AttackState
{
    public static new RangedAttackState Create(GameObject projectile, float attackSpeed, float damage)
    {
        RangedAttackState state = CreateInstance<RangedAttackState>();
        state.projectile = projectile;
        state.attackSpeed = attackSpeed;
        state.damage = damage;
        return state;
    }

    override protected void ExecuteAttack()
    {
        attackSpawnPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.5f);
        GameObject arrow = Instantiate(projectile, attackSpawnPoint, transform.rotation);
        arrow.layer = LayerMask.NameToLayer(Layer.EnemyProjectiles.ToString());
        arrow.GetComponent<Arrow>().Damage = damage;
        Arrow arrowAsset = arrow.GetComponent<Arrow>();
        arrowAsset.Shoot(player.transform.position);
    }
}
