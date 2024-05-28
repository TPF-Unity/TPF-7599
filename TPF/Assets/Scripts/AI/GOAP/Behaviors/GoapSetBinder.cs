using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace AI.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GoapSetBinder : MonoBehaviour
    {
        private GoapRunnerBehaviour goapRunner;

        private void Awake()
        {
            goapRunner = FindObjectOfType<GoapRunnerBehaviour>();
            AgentBehaviour agent = GetComponent<AgentBehaviour>();
            agent.GoapSet = goapRunner.GetGoapSet("OpponentNPCSet");
        }
    }
}