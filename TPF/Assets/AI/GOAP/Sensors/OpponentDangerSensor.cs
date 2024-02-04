using System;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Misc;
using UnityEngine;

namespace AI.GOAP.Sensors
{
    public class OpponentDangerSensor : LocalWorldSensorBase
    {
        private float detectionRadius = 10f;
        private Collider[] enemyColliders = new Collider[10];

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            var foundEnemies = Physics.OverlapSphereNonAlloc(agent.transform.position, detectionRadius, enemyColliders,
                LayerMask.NameToLayer(Layer.Player.ToString()));
            float nearestDistance = enemyColliders
                .Take(foundEnemies)
                .Select(collider =>
                    Vector3.Distance(agent.transform.position,
                        collider.transform.position))
                .DefaultIfEmpty(Mathf.Infinity)
                .Min();
            float normalizedNearestDistance = Math.Abs(10 - nearestDistance) / 10;
            var unit = agent.GetComponent<Unit>();
            var hp = unit.stats.Health;
            var maxHp = unit.baseStats.Health;
            var normalizedHp = (maxHp - hp) / maxHp * 100;
            Array.Clear(enemyColliders, 0, 10);
            return new SenseValue(Mathf.FloorToInt(normalizedHp) * Mathf.FloorToInt(normalizedNearestDistance));
        }
    }
}