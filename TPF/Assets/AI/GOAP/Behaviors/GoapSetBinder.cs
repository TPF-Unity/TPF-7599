using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace AI.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GoapSetBinder : MonoBehaviour
    {
        [SerializeField] private GoapRunnerBehaviour GoapRunner;

        private void Awake()
        {
            AgentBehaviour agent = GetComponent<AgentBehaviour>();
            agent.GoapSet = GoapRunner.GetGoapSet("OpponentNPCSet");
        }
    }
}