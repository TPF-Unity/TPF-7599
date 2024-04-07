using AI.GOAP.Config;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace AI.GOAP.Actions
{
    public class MeleeAction : ActionBase<AttackData>, IInjectable
    {
        private AttackConfigSO AttackConfig;
        private NPCStats stats;

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, AttackData data)
        {
            stats = agent.GetComponent<Unit>().stats;
            data.Timer = AttackConfig.AttackDelay;
        }
        

        public override ActionRunState Perform(IMonoAgent agent, AttackData data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;

            bool shouldAttack =
                data.Target != null && Vector3.Distance(data.Target.Position, agent.transform.position) <=
                AttackConfig.MeleeAttackRadius;
            
            if (shouldAttack)
            {
                data.AnimationController.PlayAnimation(AnimationType.Attack);
                agent.transform.LookAt(data.Target.Position);
            }

            return data.Timer > 0 ? ActionRunState.Continue : ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, AttackData data)
        {
        }

        public void Inject(DependencyInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }
    }
}