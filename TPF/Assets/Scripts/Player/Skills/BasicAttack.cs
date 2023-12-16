using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private NPCStats stats;
    public GameObject bulletPrefab;

    private float _lastShootTime;
    
    void Start()
    {
        _lastShootTime = 0f;
        stats = GetComponent<Unit>().stats;
    }

    private bool IsOnCooldown(float AttackSpeed)
    {
        return Time.time - _lastShootTime < 1f / AttackSpeed;
    }

    public void Execute(string layerName, Vector3 attackSpawnPosition, Vector3 targetPoint, float attackSpeed)
    {
        if (!IsOnCooldown(attackSpeed))
        {
            GameObject bullet = Instantiate(bulletPrefab, attackSpawnPosition, Quaternion.identity);
            bullet.layer = LayerMask.NameToLayer(layerName);
            bullet.GetComponent<Bullet>().Damage = stats.Damage;
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Shoot(targetPoint);
            _lastShootTime = Time.time;
        }

    }
}
