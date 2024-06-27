using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskEscape : Node {

    readonly ItemInfo itemInfo;
    private readonly PatrolWaypoint[] waypoints;

    public TaskEscape(PatrolWaypoint[] waypoints, ItemInfo itemInfo) {
        this.waypoints = waypoints;
        this.itemInfo = itemInfo;
    }

    public override NodeState Evaluate() {
        List<Transform> enemies = (List<Transform>) GetData(BTContextKey.Enemies);

        if (enemies != null && enemies.Count > 0) {
            Transform transform = (Transform) GetData(BTContextKey.Transform);
            NavMeshAgent navMeshAgent = (NavMeshAgent) GetData(BTContextKey.NavMeshAgent);

            Vector3 averageEnemyPosition = Vector3.zero;
            foreach (Transform enemy in enemies) {
                averageEnemyPosition += enemy.position;
            }
            averageEnemyPosition /= enemies.Count;
            
            Vector3 escapeDirection = (transform.position - averageEnemyPosition).normalized;

            PatrolWaypoint bestWaypoint = null;
            float bestAlignment = -1f;

            foreach (PatrolWaypoint waypoint in waypoints)
            {
                Vector3 directionToWaypoint = (waypoint.transform.position - transform.position).normalized;
                float alignment = Vector3.Dot(escapeDirection, directionToWaypoint);

                foreach (Transform enemy in enemies) {
                    if (Vector3.Distance(enemy.position, waypoint.transform.position) < itemInfo.detectionRange) {
                        continue;
                    }
                }
                if (Vector3.Distance(transform.position, waypoint.transform.position) < 2f) {
                    continue;
                }

                if (alignment > bestAlignment)
                {
                    bestAlignment = alignment;
                    bestWaypoint = waypoint;
                }
            }

            if (bestWaypoint != null) {
                navMeshAgent.SetDestination(bestWaypoint.transform.position);
                Node root = (Node) GetData(BTContextKey.Root);
                root.SetData(BTContextKey.Escaping, true);
                root.SetData(BTContextKey.EscapeRoute, bestWaypoint.transform.position);
            }

            state = NodeState.RUNNING;
            return state;
        } else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}