using UnityEngine;

namespace AI.GOAP.Behaviors
{
    public class RangedAttackBehavior : MonoBehaviour
    {
        //[field: SerializeField] private Transform SpawnLocation;

        public delegate void SpawnBulletEvent(Transform attackSpawnPoint);

        public event SpawnBulletEvent OnSpawnBullet;

        public GameObject currentTarget;

        public Transform attackSpawnPoint;

        public void BeginAttack(int _)
        {
            OnSpawnBullet?.Invoke(attackSpawnPoint);
        }
    }
}