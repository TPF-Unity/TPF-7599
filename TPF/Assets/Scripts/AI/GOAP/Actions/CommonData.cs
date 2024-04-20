using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;

namespace AI.GOAP.Actions
{
    public class CommonData : IActionData
    {
        [GetComponent] public NPCAnimationController AnimationController { get; set; }
        public ITarget Target { get; set; }
        public float Timer { get; set; }
    }
}