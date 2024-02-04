using System;
using AI.GOAP.Actions;
using AI.GOAP.Debuggers;
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
            builder.SetAgentDebugger<AgentDebugger>();

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
            builder.AddGoal<GoToDoorGoal>().AddCondition<KeysPicked>(Comparison.GreaterThanOrEqual, 1);
            builder.AddGoal<SurviveGoal>().AddCondition<OpponentDanger>(Comparison.SmallerThanOrEqual, 0);
        }

        private void BuildActions(GoapSetBuilder builder)
        {
            builder.AddAction<WanderAction>().SetTarget<WanderTarget>().AddEffect<IsWandering>(EffectType.Increase)
                .SetBaseCost(5).SetInRange(2);

            builder.AddAction<CollectKeysAction>().SetTarget<KeyTarget>().AddEffect<KeysRemaining>(EffectType.Decrease)
                .SetBaseCost(1).SetInRange(Injector.KeysConfig.KeySearchRadius);

            builder.AddAction<GoToDoorAction>().SetTarget<DoorTarget>().AddEffect<KeysPicked>(EffectType.Increase)
                .SetBaseCost(1).SetInRange(1);

            builder.AddAction<RangedAttackAction>()
                .AddCondition<PlayerDistance>(Comparison.SmallerThanOrEqual,
                    Mathf.FloorToInt(Injector.AttackConfig.RangedAttackRadius))
                .AddCondition<PlayerDistance>(Comparison.GreaterThanOrEqual,
                    Mathf.FloorToInt(Injector.AttackConfig.MeleeAttackRadius))
                .SetTarget<PlayerTarget>().AddEffect<PlayerHealth>(EffectType.Decrease)
                .AddEffect<OpponentDanger>(EffectType.Increase)
                .SetBaseCost(Injector.AttackConfig.RangedAttackCost)
                .SetInRange(Injector.AttackConfig.SensorRadius)
                .SetMoveMode(ActionMoveMode.PerformWhileMoving);

            builder.AddAction<MeleeAction>().SetTarget<PlayerTarget>().AddEffect<PlayerHealth>(EffectType.Decrease)
                .SetBaseCost(Injector.AttackConfig.MeleeAttackCost).SetInRange(Injector.AttackConfig.SensorRadius);

            builder.AddAction<KeepDistanceAction>().SetTarget<PlayerTarget>()
                .AddEffect<OpponentDanger>(EffectType.Decrease).SetBaseCost(1)
                .SetInRange(10);
        }

        private void BuildSensors(GoapSetBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>().SetTarget<WanderTarget>();
            builder.AddTargetSensor<PlayerTargetSensor>().SetTarget<PlayerTarget>();
            builder.AddTargetSensor<KeyTargetSensor>().SetTarget<KeyTarget>();
            builder.AddTargetSensor<DoorTargetSensor>().SetTarget<DoorTarget>();
            builder.AddWorldSensor<PlayerDistanceSensor>().SetKey<PlayerDistance>();
            builder.AddWorldSensor<KeysRemainingSensor>().SetKey<KeysRemaining>();
            builder.AddWorldSensor<KeysPickedSensor>().SetKey<KeysPicked>();
            builder.AddWorldSensor<OpponentDangerSensor>().SetKey<OpponentDanger>();
        }
    }
}