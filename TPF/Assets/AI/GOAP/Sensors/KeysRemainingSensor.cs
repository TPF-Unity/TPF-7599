using AI.GOAP.Behaviors;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace AI.GOAP.Sensors
{
    public class KeysRemainingSensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            return new SenseValue(Mathf.FloorToInt(references.GetCachedComponent<KeyCollectorBehavior>().KeysRemaining));
        }
    }
}