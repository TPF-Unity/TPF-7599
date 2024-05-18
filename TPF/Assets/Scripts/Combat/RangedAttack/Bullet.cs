using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public DamageLayerMapping damageLayerMapping;
    public LayerMask notShootableLayer;
    public float maxTravelDistance = 10f;
    private Vector3 startPosition;
    private Rigidbody rigidBody;
    private float damage;
    public PlayerController source;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 targetPoint)
    {
        Vector3 direction = targetPoint - transform.position;
        direction.y = 0;
        direction.Normalize();
        rigidBody.velocity = direction * 30.0f;
    }

    public void ShootAtDirection(Vector3 direction)
    {
        rigidBody.velocity = direction * 30.0f;
    }

    private void OnTriggerEnter(Collider targetCollider)
    {
        string attackerLayer = LayerMask.LayerToName(gameObject.layer);
        string targetLayer = LayerMask.LayerToName(targetCollider.gameObject.layer);
        if (damageLayerMapping.CanDamage(attackerLayer, targetLayer))
        {
            if (targetCollider.gameObject.TryGetComponent(out Unit target))
            {
                target.TakeDamageFrom(damage, source);
                Destroy(gameObject);
            }
        }
        else
        {
            if (notShootableLayer == (notShootableLayer | (1 << targetCollider.gameObject.layer)))
            {
                Destroy(gameObject);
            }
        }
    }

    public float Damage
    {
        set => damage = value;
    }
}