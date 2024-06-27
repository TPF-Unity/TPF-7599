using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private Vector3 walkPoint;
    private bool walkPointSet;

    private NavMeshAgent agent;
    private NPCAnimationController animationController;

    private GameObject[] patrolPoints;

    public static PatrolState Create()
    {
        PatrolState state = CreateInstance<PatrolState>();
        return state;
    }

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
        var checkedPatrolPoints = new HashSet<Vector3>();
        do
        {
            int index = Random.Range(0, patrolPoints.Length);
            walkPoint = patrolPoints[index].transform.position;
            distanceToWalkPoint = fsm.transform.position - walkPoint;
            checkedPatrolPoints.Add(walkPoint);
        } while (distanceToWalkPoint.magnitude < 1f && checkedPatrolPoints.Count < patrolPoints.Length);

        walkPointSet = true;
    }
}