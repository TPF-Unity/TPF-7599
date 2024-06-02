using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskKeepDistance : Node {

    public TaskKeepDistance() { }

    public override NodeState Evaluate() {
        List<Transform> enemies = (List<Transform>) GetData(BTContextKey.Enemies);

        if (enemies != null && enemies.Count > 0) {
            Transform transform = (Transform) GetData(BTContextKey.Transform);
            NavMeshAgent navMeshAgent = (NavMeshAgent) GetData(BTContextKey.NavMeshAgent);

            //! calculate best escape direction
            navMeshAgent.SetDestination(new(0, 0, 0));

            state = NodeState.RUNNING;
            return state;
        } else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}