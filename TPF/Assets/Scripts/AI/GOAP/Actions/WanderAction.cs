using AI.GOAP;
using AI.GOAP.Actions;
using AI.GOAP.Config;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace AI.GOAP.Factories
{
    public class WanderAction : ActionBase<CommonData>, IInjectable
    {
        private WanderConfigSO WanderConfig;
        
        public override void Created() {}

        public override void Start(IMonoAgent agent, CommonData data)
        {
            data.Timer = Random.Range(WanderConfig.WaitRangeBetweenWanders.x, WanderConfig.WaitRangeBetweenWanders.y);
            data.AnimationController.PlayAnimation(AnimationType.Walk);
        }

        public override ActionRunState Perform(IMonoAgent agent, CommonData data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;

            return data.Timer > 0 ? ActionRunState.Continue : ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, CommonData data) {}
        
        public void Inject(DependencyInjector injector)
        {
            WanderConfig = injector.WanderConfig;
        }
    }
}