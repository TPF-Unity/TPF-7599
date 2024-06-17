using System;
using System.Collections.Generic;
using System.Linq;
using AI.GOAP.Behaviors;
using AI.GOAP.Config;
using AI.GOAP.Scripts;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace AI.GOAP.Sensors
{
    public class DoorTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private DoorConfigSO DoorsConfig;

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var moveBehaviour = references.GetCachedComponent<AgentMoveBehavior>();
            Waypoint doorPosition = GetUnvisitedWaypoint(moveBehaviour.doorWaypoints, agent.transform.position);
            return new PositionTarget(doorPosition.Position);
        }
        
        private Waypoint GetUnvisitedWaypoint(List<Waypoint> waypoints, Vector3 currentPosition)
        {
            return waypoints
                .OrderBy(waypoint => waypoint.Visited)
                .ThenBy(waypoint => GetDistance(waypoint.Position, currentPosition))
                .FirstOrDefault();
        }
    
        private float GetDistance(Vector3 waypointPosition, Vector3 currentPosition)
        {
            return Vector3.Distance(waypointPosition, currentPosition);
        }


        public void Inject(DependencyInjector injector)
        {
            DoorsConfig = injector.DoorsConfig;
        }
    }
}