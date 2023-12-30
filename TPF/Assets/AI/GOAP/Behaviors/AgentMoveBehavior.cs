using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace AI.GOAP.Behaviors
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class AgentMoveBehavior : MonoBehaviour
    {
        private NavMeshAgent NavMeshAgent;
        private Animator Animator;
        private AgentBehaviour AgentBehavior;
        private ITarget CurrentTarget;

        [SerializeField] private float MinMoveDistance = 0.25f;
        private Vector3 LastPosition;
        

        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            AgentBehavior = GetComponent<AgentBehaviour>();
        }

        private void OnEnable()
        {
            AgentBehavior.Events.OnTargetChanged += EventsOnTargetChanged;
        }

        private void OnDisable()
        {
            AgentBehavior.Events.OnTargetChanged -= EventsOnTargetChanged;
        }

        private void EventsOnTargetChanged(ITarget target, bool inrange)
        {
            CurrentTarget = target;
            LastPosition = CurrentTarget.Position;
            NavMeshAgent.SetDestination(target.Position);
        }

        private void Update()
        {
            if (CurrentTarget == null)
            {
                return;
            }

            if (MinMoveDistance <= Vector3.Distance(CurrentTarget.Position, LastPosition))
            {
                LastPosition = CurrentTarget.Position;
                NavMeshAgent.SetDestination(CurrentTarget.Position);
            }
        }
    }
}