using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (gameObject.layer != collider.gameObject.layer)
        {
            if (collider.gameObject.TryGetComponent(out ShootingTarget target))
            {
                target.TakeDamage(10);
                Destroy(gameObject);
            }
        }
    }
}
