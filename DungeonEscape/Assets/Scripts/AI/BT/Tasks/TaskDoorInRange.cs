using System;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class TaskDoorInRange : Node {
    readonly Collider[] Colliders = new Collider[3];
    readonly ItemInfo itemInfo;

    public TaskDoorInRange(ItemInfo itemInfo) {
        this.itemInfo = itemInfo;
    }

    public override NodeState Evaluate() {
        Array.Clear(Colliders, 0, 3);
        Transform transform = (Transform) GetData(BTContextKey.Transform);
        int results = Physics.OverlapSphereNonAlloc(transform.position, itemInfo.detectionRange, Colliders, itemInfo.layer);

        if (results > 0) {
            parent.SetData(BTContextKey.Door, Colliders[0].transform);

            List<Transform> knownDoors = (List<Transform>) GetData(BTContextKey.KnownDoors);
            knownDoors ??= new();
            knownDoors.Add(Colliders[0].transform);
            ((Node) GetData(BTContextKey.Root)).SetData(BTContextKey.KnownDoors, knownDoors);

            state = NodeState.SUCCESS;
            return state;
        } else {
            parent.ClearData(BTContextKey.Door);
            state = NodeState.FAILURE;
            return state;
        }
    }
}