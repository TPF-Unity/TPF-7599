using System;
using AI.GOAP.Actions;
using AI.GOAP.Goals;
using AI.GOAP.Sensors;
using AI.GOAP.Targets;
using AI.GOAP.WorldKeys;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;
using UnityEngine;

namespace AI.GOAP.Factories
{
    [RequireComponent(typeof(DependencyInjector))]
    public class GoapSetConfigFactory : GoapSetFactoryBase
    {
        private DependencyInjector Injector;

        public override IGoapSetConfig Create()
        {
            Injector = GetComponent<DependencyInjector>();
            GoapSetBuilder builder = new("OpponentNPCSet");

            BuildGoals(builder);
            BuildActions(builder);
            BuildSensors(builder);

            return builder.Build();
        }

        private void BuildGoals(GoapSetBuilder builder)
        {
            builder.AddGoal<WanderGoal>().AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);
            builder.AddGoal<KillPlayerGoal>().AddCondition<PlayerHealth>(Comparison.SmallerThanOrEqual, 0);
            builder.AddGoal<CollectKeysGoal>()
                .AddCondition<KeysRemaining>(Comparison.SmallerThanOrEqual, 0);
        }

        private void BuildActions(GoapSetBuilder builder)
        {
            builder.AddAction<WanderAction>().SetTarget<WanderTarget>().AddEffect<IsWandering>(EffectType.Increase)
                .SetBaseCost(5).SetInRange(10);

            builder.AddAction<CollectKeysAction>().SetTarget<KeyTarget>().AddEffect<KeysRemaining>(EffectType.Decrease)
                .SetBaseCost(1).SetInRange(Injector.KeysConfig.KeySearchRadius);

            builder.AddAction<RangedAttackAction>()
                .AddCondition<PlayerDistance>(Comparison.SmallerThanOrEqual,
                    Mathf.FloorToInt(Injector.AttackConfig.RangedAttackRadius))
                .AddCondition<PlayerDistance>(Comparison.GreaterThanOrEqual,
                    Mathf.FloorToInt(Injector.AttackConfig.MeleeAttackRadius))
                .SetTarget<PlayerTarget>().AddEffect<PlayerHealth>(EffectType.Decrease)
                .SetBaseCost(Injector.AttackConfig.RangedAttackCost)
                .SetInRange(Injector.AttackConfig.SensorRadius)
                .SetMoveMode(ActionMoveMode.PerformWhileMoving);

            builder.AddAction<MeleeAction>().SetTarget<PlayerTarget>().AddEffect<PlayerHealth>(EffectType.Decrease)
                .SetBaseCost(Injector.AttackConfig.MeleeAttackCost).SetInRange(Injector.AttackConfig.SensorRadius);
        }

        private void BuildSensors(GoapSetBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>().SetTarget<WanderTarget>();
            builder.AddTargetSensor<PlayerTargetSensor>().SetTarget<PlayerTarget>();
            builder.AddTargetSensor<KeyTargetSensor>().SetTarget<KeyTarget>();
            builder.AddWorldSensor<PlayerDistanceSensor>().SetKey<PlayerDistance>();
            builder.AddWorldSensor<KeysRemainingSensor>().SetKey<KeysRemaining>();
        }
    }
}