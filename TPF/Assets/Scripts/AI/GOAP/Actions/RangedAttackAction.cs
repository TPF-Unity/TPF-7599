using AI.GOAP.Behaviors;
using AI.GOAP.Config;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Misc;
using UnityEngine;
using UnityEngine.Pool;

namespace AI.GOAP.Actions
{
    public class RangedAttackAction : ActionBase<AttackData>, IInjectable
    {
        private AttackConfigSO attackConfig;
        private ObjectPool<GameObject> pool;
        private NPCStats stats;
        private AttackData currentTargetData;

        public override void Created()
        {
            pool = new ObjectPool<GameObject>(CreateObject);
        }

        public override void Start(IMonoAgent agent, AttackData data)
        {
            stats = agent.GetComponent<Unit>().stats;
            data.Timer = attackConfig.AttackDelay;
            data.RangedAttackBehavior.OnSpawnBullet += RangedAttackBehaviorOnRangedAttack;
        }

        private void RangedAttackBehaviorOnRangedAttack(Vector3 spawnLocation)
        {
            if (currentTargetData == null)
            {
                return;
            }

            GameObject bullet = pool.Get();
            Bullet instance = bullet.GetComponent<Bullet>();
            instance.Damage = stats.Damage;
            instance.gameObject.layer = LayerMask.NameToLayer(Layer.PlayerProjectiles.ToString());
            var bulletTransform = instance.transform;
            bulletTransform.position = spawnLocation;
            instance.Shoot(currentTargetData.Target.Position);
        }

        private GameObject CreateObject()
        {
            return Object.Instantiate(attackConfig.Bullet);
        }

        public override ActionRunState Perform(IMonoAgent agent, AttackData data, ActionContext context)
        {
            if (data.Target == null)
            {
                return ActionRunState.Stop;
            }

            currentTargetData = data;
            data.Timer -= context.DeltaTime;

            bool shouldAttack =
                data.Target != null && Vector3.Distance(data.Target.Position, agent.transform.position) <=
                attackConfig.RangedAttackRadius;

            if (shouldAttack)
            {
                agent.transform.LookAt(data.Target.Position);
                data.AnimationController.PlayAnimation(AnimationType.Attack);
            }

            return data.Timer > 0 ? ActionRunState.Continue : ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, AttackData data)
        {
            data.RangedAttackBehavior.OnSpawnBullet -= RangedAttackBehaviorOnRangedAttack;
        }

        public void Inject(DependencyInjector injector)
        {
            attackConfig = injector.AttackConfig;
        }
    }
}