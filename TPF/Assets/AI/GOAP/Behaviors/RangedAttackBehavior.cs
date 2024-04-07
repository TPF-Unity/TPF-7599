using UnityEngine;

namespace AI.GOAP.Behaviors
{
    public class RangedAttackBehavior : MonoBehaviour
    {
        //[field: SerializeField] private Transform SpawnLocation;

        public delegate void SpawnBulletEvent(Vector3 spawnLocation);

        public event SpawnBulletEvent OnSpawnBullet;

        public GameObject currentTarget;

        public void BeginAttack(int _)
        {
            Vector3 attackSpawnPoint =
                new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                transform.forward * 1.0f;
            // TODO: Add attackSpawn on each unit

            OnSpawnBullet?.Invoke(attackSpawnPoint);
        }
    }
}