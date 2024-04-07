using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Misc;
using UnityEngine;
using UnityEngine.AI;

namespace AI.GOAP.Actions
{
    public class KeepDistanceAction : ActionBase<CommonData>, IInjectable
    {
        private Collider[] enemyColliders = new Collider[10];
        private int numberOfSectors = 8;
        private int detectionRadius = 13;
        private float escapeDistance = 4f;
        private float timer = 2f;
        private Vector3 lastTargetPosition;

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, CommonData data)
        {
            data.Timer = timer;
            data.AnimationController.PlayAnimation(AnimationType.Walk);
        }

        Vector3 FindEscapeDirection(Transform transform, int pos = 0)
        {
            int enemyCount = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, enemyColliders,
                LayerMask.NameToLayer(Layer.Player.ToString()));
            int[] sectorCounts = new int[numberOfSectors];
            float sectorAngle = 360f / numberOfSectors;

            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 directionToEnemy = enemyColliders[i].transform.position - transform.position;
                float angleWithForward = Vector3.SignedAngle(transform.forward, directionToEnemy, Vector3.up);
                int sectorIndex = (Mathf.FloorToInt((angleWithForward + 180f) / sectorAngle)) % numberOfSectors;
                sectorCounts[sectorIndex]++;
            }

            var leastDenseSectors = sectorCounts
                .Select((count, index) =>
                    new { Count = count, Index = index })
                .OrderBy(x => x.Count);

            Vector3 possibleEscapeTarget = Vector3.zero;

            foreach (var sector in leastDenseSectors)
            {
                float escapeAngle =
                    sectorAngle * sector.Index - 180f + sectorAngle / 2;
                var direction = Quaternion.Euler(0, escapeAngle, 0) * Vector3.forward;
                possibleEscapeTarget = transform.position + direction * escapeDistance;
                if (NavMesh.SamplePosition(possibleEscapeTarget, out NavMeshHit hit, 1, NavMesh.AllAreas))
                {
                    break;
                }
            }

            return possibleEscapeTarget;
        }

        private float minDistanceToRecalculate = 5f; // Minimum distance to travel before recalculating

        public override ActionRunState Perform(IMonoAgent agent, CommonData data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;

            var navMesh = agent.GetComponent<NavMeshAgent>();

            if (!navMesh.pathPending && navMesh.remainingDistance < minDistanceToRecalculate)
            {
                Vector3 escapeTarget = FindEscapeDirection(agent.transform);
                navMesh.SetDestination(escapeTarget);
            }

            return data.Timer > 0 ? ActionRunState.Continue : ActionRunState.Stop;
        }


        public override void End(IMonoAgent agent, CommonData data)
        {
        }

        public void Inject(DependencyInjector injector)
        {
        }
    }
}