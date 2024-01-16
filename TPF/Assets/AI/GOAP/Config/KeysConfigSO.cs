using UnityEngine;

namespace AI.GOAP.Config
{
    [CreateAssetMenu(menuName = "AI/Keys Config", fileName = "Keys Config", order = 1)]
    public class KeysConfigSO: ScriptableObject
    {
        public float RequiredKeys = 2;
        public float KeySearchRadius = 11f;
        public LayerMask KeyLayer;
    }
}