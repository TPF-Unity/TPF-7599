using AI.GOAP.Config;
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace AI.GOAP.Behaviors
{
    public class KeyCollectorBehavior : MonoBehaviour
    {
        private float modelSizeAdjustment = 0.3f;
        [field: SerializeField] public float KeysRemaining { get; set; }

        [SerializeField] public KeysConfigSO KeysConfig;

        public bool KeyNotFound;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, KeysConfig.KeySearchRadius * modelSizeAdjustment);
        }

        public void CollectKey()
        {
            KeysRemaining -= 1;
        }

        private void Awake()
        {
            KeysRemaining = KeysConfig.RequiredKeys;
        }
    }
}