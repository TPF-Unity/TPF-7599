using UnityEngine;

namespace AI.GOAP.Config
{
    [CreateAssetMenu(menuName = "AI/Doors Config", fileName = "Doors Config", order = 1)]
    public class DoorConfigSO: ScriptableObject
    {
        public float DoorActionTimer = 2.0f;
        public LayerMask DoorLayer;
    }
}