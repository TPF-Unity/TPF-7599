using UnityEngine;

namespace AI.GOAP.Behaviors
{
    public class RangedAttackBehavior : MonoBehaviour
    {
        //[field: SerializeField] private Transform SpawnLocation;

        public delegate void SpawnBulletEvent(Vector3 spawnLocation, Vector3 forward);

        public event SpawnBulletEvent OnSpawnBullet;

        private Transform player;

        public void Awake()
        {
            player = GameObject.Find("Player").transform;
        }

        public void BeginAttack(int _)
        {
            Vector3 attackSpawnPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                                       transform.forward * 1.0f;
            // TODO: Add attackSpawn on each unit
            Vector3 yOffset = new Vector3(0,0.5f,0);
            var direction = (player.position + yOffset - attackSpawnPoint);
            OnSpawnBullet?.Invoke(attackSpawnPoint, direction.normalized);
        }
    }
}