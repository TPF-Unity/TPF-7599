using System;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class TaskCheckPowerUpInRange : Node {
    readonly Collider[] Colliders = new Collider[3];
    readonly ItemInfo itemInfo;

    public TaskCheckPowerUpInRange(ItemInfo itemInfo) {
        this.itemInfo = itemInfo;
    }

    public override NodeState Evaluate() {
        Array.Clear(Colliders, 0, 3);
        Transform transform = (Transform) GetData(BTContextKey.Transform);
        int results = Physics.OverlapSphereNonAlloc(transform.position, itemInfo.detectionRange, Colliders, itemInfo.layer);

        if (results > 0) {
            Debug.Log("SEEN POWER UP");
            parent.SetData(BTContextKey.PowerUp, Colliders[0].transform);

            state = NodeState.SUCCESS;
            return state;
        } else {
            parent.ClearData(BTContextKey.PowerUp);
            state = NodeState.FAILURE;
            return state;
        }
    }
}