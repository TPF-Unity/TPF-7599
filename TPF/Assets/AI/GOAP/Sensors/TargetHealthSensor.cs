using AI.GOAP.Actions;
using AI.GOAP.Behaviors;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace AI.GOAP.Sensors
{
    public class TargetHealthSensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            float hp = 0;
            var currentTarget = references.GetCachedComponent<RangedAttackBehavior>().currentTarget;
            if (currentTarget != null)
            {
                hp = references.GetCachedComponent<RangedAttackBehavior>().currentTarget.GetComponent<Unit>().stats
                    .Health;
            }

            Debug.Log(Mathf.FloorToInt(hp));
            return new SenseValue(Mathf.FloorToInt(hp));
        }
    }
}