using System;
using System.Collections.Generic;
using System.Linq;
using AI.GOAP.Scripts;
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
        public List<Waypoint> keyWaypoints;
        public List<Waypoint> doorWaypoints;

        private void InitializeWaypoints()
        {
            GameObject[] keySpawns = GameObject.FindGameObjectsWithTag("KeySpawn");

            foreach (var keySpawn in keySpawns)
            {
                keyWaypoints.Add(new Waypoint(keySpawn.transform.position));
            }

            GameObject[] doorSpawns = GameObject.FindGameObjectsWithTag("DoorSpawn");

            foreach (var doorSpawn in doorSpawns)
            {
                keyWaypoints.Add(new Waypoint(doorSpawn.transform.position));
            }
        }

        private void Start()
        {
            keyWaypoints = new List<Waypoint>();
            if (GameManager.instance && (!keyWaypoints.Any() || !doorWaypoints.Any()))
            {
                InitializeWaypoints();
            }
        }

        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            AgentBehavior = GetComponent<AgentBehaviour>();
        }

        private void OnEnable()
        {
            AgentBehavior.Events.OnTargetChanged += EventsOnTargetChanged;
            AgentBehavior.Events.OnTargetInRange += EventsOnTargetInRange;
        }

        private void OnDisable()
        {
            AgentBehavior.Events.OnTargetChanged -= EventsOnTargetChanged;
            AgentBehavior.Events.OnTargetInRange -= EventsOnTargetInRange;
        }

        private void EventsOnTargetInRange(ITarget target)
        {
            var targetKeyWaypoint = keyWaypoints?.Where(waypoint => waypoint?.Position == target?.Position)?.ToArray();
            if (targetKeyWaypoint?.Length > 0)
            {
                targetKeyWaypoint[0].Visited = true;
            }

            var targetDoorWaypoint =
                doorWaypoints?.Where(waypoint => waypoint?.Position == target?.Position)?.ToArray();
            if (targetDoorWaypoint?.Length > 0)
            {
                targetDoorWaypoint[0].Visited = true;
            }
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
        }
    }
}