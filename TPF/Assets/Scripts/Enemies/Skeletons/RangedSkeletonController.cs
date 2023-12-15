using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeletonController : SkeletonController
{
    protected override void ExecuteAttack()
    {
        attackSpawnPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.5f);
        GameObject arrow = Instantiate(projectile, attackSpawnPoint, transform.rotation);
        arrow.layer = LayerMask.NameToLayer("EnemiesProjectiles");
        arrow.GetComponent<Arrow>().Damage = stats.Damage;
        Arrow arrowAsset = arrow.GetComponent<Arrow>();
        arrowAsset.Shoot(player.transform.position);
    }
}
