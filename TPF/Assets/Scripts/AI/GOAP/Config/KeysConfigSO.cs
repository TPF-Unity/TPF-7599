using UnityEngine;

namespace AI.GOAP.Config
{
    [CreateAssetMenu(menuName = "AI/Keys Config", fileName = "Keys Config", order = 1)]
    public class KeysConfigSO: ScriptableObject
    {
        public float KeySearchRadius = 11f;
        public float CollectKeysActionTimer = 2.0f;
        public LayerMask KeyLayer;
    }
}