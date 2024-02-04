using AI.GOAP.Behaviors;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace AI.GOAP.Sensors
{
    public class KeysPickedSensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            var keyCollectorBehavior = references.GetCachedComponent<KeyCollectorBehavior>();
            return new SenseValue(Mathf.FloorToInt(keyCollectorBehavior.KeysRemaining));
        }
    }
}