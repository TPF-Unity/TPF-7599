using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxTravelDistance = 10f;
    private Vector3 startPosition;

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

    private void OnTriggerEnter(Collider collider)
    {
        if (gameObject.layer != collider.gameObject.layer)
        {
            if (collider.gameObject.TryGetComponent(out VulnerableUnit target))
            {
                target.TakeDamage(10);
                Destroy(gameObject);
            }
        }
    }
}
