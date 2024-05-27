using System;
using BehaviourTree;
using UnityEngine;

public class TaskKeyInRange : Node {
    readonly Collider[] Colliders = new Collider[3];
    readonly ItemInfo itemInfo;

    public TaskKeyInRange(ItemInfo itemInfo) {
        this.itemInfo = itemInfo;
    }

    public override NodeState Evaluate() {
        Array.Clear(Colliders, 0, 3);
        Transform transform = (Transform) GetData(BTContextKey.Transform);
        int results = Physics.OverlapSphereNonAlloc(transform.position, itemInfo.detectionRange, Colliders, itemInfo.layer);

        if (results > 0 && Colliders[0].gameObject.GetComponent<KeyController>().CanPickUpKey(transform.gameObject)) {
            parent.SetData(BTContextKey.Key, Colliders[0].transform);

            state = NodeState.SUCCESS;
            return state;
        } else {
            ClearData(BTContextKey.Key);
            state = NodeState.FAILURE;
            return state;
        }
    }
}