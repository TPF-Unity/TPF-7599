using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public DamageLayerMapping damageLayerMapping;
    public LayerMask notShootableLayer;
    public float maxTravelDistance = 1000f;
    public Vector3 startPosition;
    private Rigidbody rigidBody;
    private float damage;
    public GameObject origin;
    public PlayerController source;

    private float BULLET_SPEED = 30f;

    void Awake()
    {
        startPosition = transform.position;
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
        rigidBody.velocity = direction * BULLET_SPEED;
    }

    public void ShootAtDirection(Vector3 direction)
    {
        rigidBody.velocity = direction * BULLET_SPEED;
    }

    private void OnTriggerEnter(Collider collider)
    {

        string attackerLayer = LayerMask.LayerToName(gameObject.layer);
        string targetLayer = LayerMask.LayerToName(collider.gameObject.layer);
        if (damageLayerMapping.CanDamage(attackerLayer, targetLayer) && origin != collider.gameObject)
        {
            if (collider.gameObject.TryGetComponent(out Unit target))
            {
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
