using System.Linq;
using AI.GOAP.Behaviors;
using AI.GOAP.Config;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace AI.GOAP.Sensors
{
    public class PlayerTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private AttackConfigSO AttackConfig;
        private Collider[] Colliders = new Collider[1];

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            if (Physics.OverlapSphereNonAlloc(agent.transform.position, AttackConfig.SensorRadius, Colliders,
                    AttackConfig.AttackableLayerMask) > 0)
            {
                Colliders = Colliders.OrderBy(collider =>
                        collider == null
                            ? int.MaxValue
                            : (collider.transform.position - agent.transform.position).sqrMagnitude)
                    .ToArray();
                var attackBehaviour = references.GetCachedComponent<RangedAttackBehavior>();
                attackBehaviour.currentTarget = Colliders[0].gameObject;
                return new TransformTarget(Colliders[0].transform);
            }

            return null;
        }

        public void Inject(DependencyInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }
    }
}