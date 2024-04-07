using UnityEngine;

namespace AI.GOAP.Config
{
    [CreateAssetMenu(menuName = "AI/Attack Config", fileName = "Attack Config", order = 1)]
    public class AttackConfigSO : ScriptableObject
    {
        public float SensorRadius = 10;
        public float MeleeAttackRadius = 1f;
        public int MeleeAttackCost = 1;
        public float AttackDelay = 1;
        public LayerMask AttackableLayerMask;
        public float RangedAttackRadius = 7;
        public GameObject Bullet;
        public int RangedAttackCost = 1;
        public float BulletSpeed = 30.0f;
        public DamageLayerMapping damageLayerMapping;
    }
}