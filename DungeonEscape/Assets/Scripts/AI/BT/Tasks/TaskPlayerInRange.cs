using System;
using AI.Sensors;
using BehaviourTree;
using UnityEngine;

public class TaskPlayerInRange : Node {
    readonly Collider[] Colliders = new Collider[3];
    readonly ItemInfo itemInfo;

    public TaskPlayerInRange(ItemInfo itemInfo) {
        this.itemInfo = itemInfo;
    }

    public override NodeState Evaluate() {
        Array.Clear(Colliders, 0, 3);
        Transform transform = (Transform) GetData(BTContextKey.Transform);
        int results = Physics.OverlapSphereNonAlloc(transform.position, itemInfo.detectionRange, Colliders, itemInfo.layer);

        if (results > 0 && Colliders[0]) {
            parent.SetData(BTContextKey.Player, Colliders[0].transform);

            state = NodeState.SUCCESS;
            return state;
        } else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}