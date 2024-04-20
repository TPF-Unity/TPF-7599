using System.Collections.Generic;
using System.Linq;
using AI.GOAP;
using AI.GOAP.Behaviors;
using AI.GOAP.Config;
using AI.GOAP.Scripts;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;
using UnityEngine.AI;

public class WanderTargetSensor : LocalTargetSensorBase, IInjectable
{
    private WanderConfigSO WanderConfig;
    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        var moveBehaviour = references.GetCachedComponent<AgentMoveBehavior>();
        Waypoint position = GetUnvisitedWaypoint(moveBehaviour.keyWaypoints, agent.transform.position);
        return new PositionTarget(position.Position);
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
        WanderConfig = injector.WanderConfig;
    }
}