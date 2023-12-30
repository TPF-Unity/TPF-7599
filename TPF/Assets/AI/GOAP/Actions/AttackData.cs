using CrashKonijn.Goap.Classes.References;
using UnityEngine;

namespace AI.GOAP.Actions
{
    public class AttackData : CommonData
    {
        [GetComponent] public Animator Animator { get; set; }
    }
}