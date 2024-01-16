using UnityEngine;

namespace AI.GOAP.Config
{
    [CreateAssetMenu(menuName = "AI/Wander Config", fileName = "Wander Config", order = 2)]
    public class WanderConfigSO : ScriptableObject
    {
        public Vector2 WaitRangeBetweenWanders = new Vector2(1, 5);
        public float WanderRadius = 5f;
    }
}