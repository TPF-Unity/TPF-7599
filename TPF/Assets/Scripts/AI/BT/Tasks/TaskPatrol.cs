using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskPatrol : Node {
    private readonly PatrolWaypoint[] waypoints;

    private bool waiting = false;
    private float time = 0f;

    public TaskPatrol(PatrolWaypoint[] waypoints) {
        this.waypoints = waypoints;
    }

    public override NodeState Evaluate() {
        if (waypoints.Length == 0) {
            state = NodeState.FAILURE;
            return state;
        }

        time += Time.deltaTime;
        if (time > 0.5f) {
            waiting = false;
        }

        if (!waiting) {
            Transform transform = (Transform) GetData(BTContextKey.Transform);
            NavMeshAgent navMeshAgent = (NavMeshAgent) GetData(BTContextKey.NavMeshAgent);
            PatrolWaypoint nextWp = waypoints.OrderBy(waypoint => waypoint.visited).ThenBy(waypoint => Vector3.Distance(waypoint.transform.position, transform.position)).FirstOrDefault();

            // Check if doors known are contained in waypoints, and if so get the closest of them
            List<Transform> knownDoors = (List<Transform>) GetData(BTContextKey.KnownDoors);
            if (knownDoors != null && knownDoors.Count() > 0 && waypoints.Any(wp => wp.transform.position == knownDoors[0].position)) {
                nextWp = new PatrolWaypoint(knownDoors.OrderBy(door => Vector3.Distance(door.position, transform.position)).FirstOrDefault());
            }

            navMeshAgent.SetDestination(nextWp.transform.position);

            if (Vector3.Distance(transform.position, nextWp.transform.position) < 1f) {
                waiting = true;
                time = 0;
                nextWp.visited = true;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}