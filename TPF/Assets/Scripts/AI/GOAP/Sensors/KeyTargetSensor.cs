using System;
using System.Linq;
using AI.GOAP.Config;
using AI.Sensors;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace AI.GOAP.Sensors
{
    public class KeyTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private KeysConfigSO KeysConfig;
        private Collider[] Colliders = new Collider[3];

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            Vector3 agentPosition = agent.transform.position;
            Array.Clear(Colliders, 0, 3);
            int hits = Physics.OverlapSphereNonAlloc(agentPosition, KeysConfig.KeySearchRadius, Colliders,
                KeysConfig.KeyLayer);
            if (hits == 0)
            {
                return null;
            }

            Colliders = Colliders.OrderBy(collider =>
                    collider == null
                        ? int.MaxValue
                        : (collider.transform.position - agent.transform.position).sqrMagnitude)
                .ToArray();

            var targetPosition = new PositionTarget(Colliders[0].transform.position);
            Array.Clear(Colliders, 0, 3);
            return targetPosition;
        }

        public void Inject(DependencyInjector injector)
        {
            KeysConfig = injector.KeysConfig;
        }
    }
}