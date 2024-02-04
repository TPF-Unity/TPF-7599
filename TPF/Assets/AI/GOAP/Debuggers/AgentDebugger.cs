using System.Text;
using AI.GOAP.Behaviors;
using AI.GOAP.Scripts;
using CrashKonijn.Goap.Interfaces;

namespace AI.GOAP.Debuggers
{
    public class AgentDebugger : IAgentDebugger
    {
        public string GetInfo(IMonoAgent agent, IComponentReference references)
        {
            var agentMove = references.GetCachedComponent<AgentMoveBehavior>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Waypoint.toString(agentMove.keyWaypoints));
            sb.AppendLine(Waypoint.toString(agentMove.doorWaypoints));
            return sb.ToString();
        }
    }
}