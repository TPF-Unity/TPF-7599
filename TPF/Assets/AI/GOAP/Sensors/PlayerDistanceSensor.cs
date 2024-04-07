using AI.GOAP.Config;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace AI.GOAP.Sensors
{
    public class PlayerDistanceSensor : LocalWorldSensorBase, IInjectable
    {
        private AttackConfigSO AttackConfig;
        private Collider[] Colliders = new Collider[1];

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            int count = Physics.OverlapSphereNonAlloc(agent.transform.position, AttackConfig.SensorRadius, Colliders,
                AttackConfig.AttackableLayerMask);

            for (int i = 0; i < count; i++)
            {
                if (Colliders[i] != null && Colliders[i].gameObject != agent.gameObject &&
                    Colliders[i].TryGetComponent(out Unit unit))
                {
                    if (AttackConfig.damageLayerMapping.CanDamage(LayerMask.LayerToName(agent.gameObject.layer),
                            LayerMask.LayerToName(Colliders[i].gameObject.layer)))
                    {
                        int distance = Mathf.CeilToInt(Vector3.Distance(agent.transform.position,
                            Colliders[i].transform.position));

                        return new SenseValue(distance);
                    }
                }
            }

            return int.MaxValue;
        }


        public void Inject(DependencyInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }
    }
}