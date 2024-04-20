using System;
using System.Collections.Generic;
using System.Text;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace AI.GOAP.Scripts

{
    [Serializable]
    public class Waypoint : ITarget
    {
        public Waypoint(Vector3 pos)
        {
            Position = pos;
            Visited = false;
        }

        public Vector3 Position { get; set; }

        public bool Visited { get; set; }

        public static String toString(List<Waypoint> waypoints)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var waypoint in waypoints)
            {
                sb.AppendLine($"Position: {waypoint.Position}, Visited: {waypoint.Visited}");
            }

            return sb.ToString();
        }
    }
}