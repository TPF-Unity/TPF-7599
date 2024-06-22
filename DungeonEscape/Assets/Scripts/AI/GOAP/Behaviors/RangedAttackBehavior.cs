using UnityEngine;

namespace AI.GOAP.Behaviors
{
    public class RangedAttackBehavior : MonoBehaviour
    {
        //[field: SerializeField] private Transform SpawnLocation;

        public delegate void SpawnBulletEvent(Vector3 attackSpawnPoint, GameObject origin);

        public event SpawnBulletEvent OnSpawnBullet;

        public GameObject currentTarget;

        public Transform attackSpawnPoint;

        public AudioClip attackSound;

        public void BeginAttack(int _)
        {
            if (attackSound != null)
            {
                AudioSource.PlayClipAtPoint(attackSound, transform.position);
            }
            OnSpawnBullet?.Invoke(attackSpawnPoint.transform.position, gameObject);
        }
    }
}