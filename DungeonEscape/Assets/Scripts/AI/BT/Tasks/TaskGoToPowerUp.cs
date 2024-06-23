using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskGoToPowerUp : Node {

    public TaskGoToPowerUp() { }

    public override NodeState Evaluate() {
        Transform target = (Transform) GetData(BTContextKey.PowerUp);

        if (target != null) {
            Transform transform = (Transform) GetData(BTContextKey.Transform);
            NavMeshAgent navMeshAgent = (NavMeshAgent) GetData(BTContextKey.NavMeshAgent);

            if (Vector3.Distance(transform.position, target.position) > 0.1f) {
                navMeshAgent.SetDestination(target.position);
            }

            state = NodeState.RUNNING;
            return state;
        } else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}