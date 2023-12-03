using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.layer != collision.gameObject.layer)
        {
            if (collision.gameObject.TryGetComponent(out ShootingTarget target))
            {
                target.TakeDamage(10);
                Destroy(gameObject);
            }
        }
    }
}
