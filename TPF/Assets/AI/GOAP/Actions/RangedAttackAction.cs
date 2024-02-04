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
    public class RangedAttackAction : ActionBase<RangedAttackAction.Data>, IInjectable
    {
        private AttackConfigSO attackConfig;
        private ObjectPool<Bullet> pool;
        private NPCStats stats;

        public override void Created()
        {
            pool = new ObjectPool<Bullet>(CreateObject);
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = attackConfig.AttackDelay;
            stats = agent.GetComponent<Unit>().stats;
            data.RangedAttackBehavior.OnSpawnBullet += RangedAttackBehaviorOnRangedAttack;
        }

        private void RangedAttackBehaviorOnRangedAttack(Vector3 spawnLocation, Vector3 forward)
        {
            Bullet instance = pool.Get();
            instance.Damage = stats.Damage;
            instance.gameObject.layer = LayerMask.NameToLayer(Layer.EnemyProjectiles.ToString());
            var bulletTransform = instance.transform;
            bulletTransform.position = spawnLocation;
            bulletTransform.forward = forward;
            var bulletRigidBody = instance.GetComponent<Rigidbody>();
            bulletRigidBody.velocity = forward * attackConfig.BulletSpeed;
        }

        private Bullet CreateObject()
        {
            return Object.Instantiate(attackConfig.Bullet);
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
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

        public override void End(IMonoAgent agent, Data data)
        {
            data.RangedAttackBehavior.OnSpawnBullet -= RangedAttackBehaviorOnRangedAttack;
        }

        public void Inject(DependencyInjector injector)
        {
            attackConfig = injector.AttackConfig;
        }

        public class Data : AttackData
        {
            [GetComponent] public RangedAttackBehavior RangedAttackBehavior { get; set; }
        }
    }
}