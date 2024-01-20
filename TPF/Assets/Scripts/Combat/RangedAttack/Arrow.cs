using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public DamageLayerMapping damageLayerMapping;
    public LayerMask notShootableLayer;
    public float maxTravelDistance = 10f;
    private Vector3 startPosition;
    private Rigidbody rigidBody;
    private float damage;

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
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        } else {
            if (notShootableLayer == (notShootableLayer | (1 << collider.gameObject.layer)))
            {
                Destroy(gameObject);
            }
        }
    }

    public float Damage { set => damage = value; }
}
