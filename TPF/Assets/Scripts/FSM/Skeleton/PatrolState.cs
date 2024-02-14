using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/State/Patrol")]
public class PatrolState : State
{
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    private NavMeshAgent agent;
    private NPCAnimationController animationController;

    private GameObject[] patrolPoints;

    public override void EnterState(FSM fsm)
    {
        walkPointSet = false;
        agent = fsm.GetComponent<NavMeshAgent>();
        animationController = fsm.GetComponent<NPCAnimationController>();
        patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
    }

    protected override void ExecuteState(FSM fsm)
    {
        animationController.PlayAnimation(AnimationType.Walk);

        if (!walkPointSet)
        {
            SearchWalkPoint(fsm);
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = fsm.transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint(FSM fsm)
    {
        Vector3 distanceToWalkPoint = fsm.transform.position - walkPoint;

        while (distanceToWalkPoint.magnitude < 1f) {
            int index = Random.Range(0, patrolPoints.Length);
            walkPoint = patrolPoints[index].transform.position;
            distanceToWalkPoint = fsm.transform.position - walkPoint;
        }

         walkPointSet = true;
    }
}
