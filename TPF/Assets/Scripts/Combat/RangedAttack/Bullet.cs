using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public DamageLayerMapping damageLayerMapping;
    public LayerMask notShootableLayer;
    public float maxTravelDistance = 1000f;
    private Vector3 startPosition;
    private Rigidbody rigidBody;
    private float damage;
    public PlayerController source;

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
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = direction * 30.0f;
    }

    private void OnTriggerEnter(Collider collider)
    {

        string attackerLayer = LayerMask.LayerToName(gameObject.layer);
        string targetLayer = LayerMask.LayerToName(collider.gameObject.layer);
        if (damageLayerMapping.CanDamage(attackerLayer, targetLayer))
        {
            if (collider.gameObject.TryGetComponent(out Unit target))
            {
                // Debug.Log("damage");
                // Debug.Log(damage);
                target.TakeDamageFrom(damage, source);
                Destroy(gameObject);
            }
        }
        else
        {
            if (notShootableLayer == (notShootableLayer | (1 << collider.gameObject.layer)))
            {
                Destroy(gameObject);
            }
        }
    }

    public float Damage { set => damage = value; }
}
