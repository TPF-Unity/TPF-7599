using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeletonController : SkeletonController
{
    protected override void ExecuteAttack(){
        attackSpawnPoint = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z + 0.5f);
        GameObject slash = Instantiate(projectile, attackSpawnPoint, transform.rotation);
        slash.layer = LayerMask.NameToLayer("EnemiesProjectiles");
        slash.GetComponent<Slash>().Damage = stats.Damage;
        Slash slashAsset = slash.GetComponent<Slash>();
        slashAsset.Execute();
    }
}
