﻿using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Observers
{
    public class ConditionObserver : ConditionObserverBase
    {
        public override bool IsMet(ICondition condition)
        {
            return this.WorldData.IsTrue(condition.WorldKey, condition.Comparison, condition.Amount);
        }
    }
}