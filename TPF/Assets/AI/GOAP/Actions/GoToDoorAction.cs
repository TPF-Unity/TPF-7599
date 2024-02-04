using AI.GOAP.Config;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;


namespace AI.GOAP.Actions
{
    public class GoToDoorAction : ActionBase<CommonData>, IInjectable
    {
        private DoorConfigSO DoorsConfigSO;
        private ITarget doorPosition;
        private NavMeshAgent NavMeshAgent;
        

        public override void Created()
        {

        }

        public override void Start(IMonoAgent agent, CommonData data)
        {
            data.Timer = DoorsConfigSO.DoorActionTimer;
            data.AnimationController.PlayAnimation(AnimationType.Walk);
        }

        public override ActionRunState Perform(IMonoAgent agent, CommonData data, ActionContext context)
        {
            if (doorPosition == null)
            {
                var navMesh = agent.GetComponent<NavMeshAgent>();
                navMesh.SetDestination(data.Target.Position);
                doorPosition = data.Target;
            }
            
            data.Timer -= context.DeltaTime;

            if (data.Timer > 0)
            {
                return ActionRunState.Continue;
            }

            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, CommonData data)
        {
        }

        public void Inject(DependencyInjector injector)
        {
            DoorsConfigSO = injector.DoorsConfig;
        }
    }
}