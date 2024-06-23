using System;
using AI.GOAP.Config;
using BehaviourTree;
using UnityEngine;

public class TaskHPLowerThan : Node {
    private readonly float comparisonHP;

    public TaskHPLowerThan(float comparisonHP) {
        this.comparisonHP = comparisonHP;
    }

    public override NodeState Evaluate() {
        NPCStats stats = ((Unit) GetData(BTContextKey.Unit)).stats;

        if (stats.Health / stats.MaxHealth < comparisonHP) {
            state = NodeState.SUCCESS;
            return state;
        } else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}