using System;
using AI.GOAP.Config;
using BehaviourTree;
using UnityEngine;

public class TaskKeysCollected : Node {
    public TaskKeysCollected() { }

    public override NodeState Evaluate() {
        Transform transform = (Transform) GetData(BTContextKey.Transform);
        KeyProgressionManager keyProgressionManager = transform.GetComponent<KeyProgressionManager>();
        if (keyProgressionManager != null && keyProgressionManager.HasAllKeys()) {

            state = NodeState.SUCCESS;
            return state;
        } else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}