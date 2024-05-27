using System;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;

public class TaskEnemiesInRange : Node {
    readonly Collider[] Colliders = new Collider[3];
    readonly ItemInfo itemInfo;

    public TaskEnemiesInRange(ItemInfo itemInfo) {
        this.itemInfo = itemInfo;
    }

    public override NodeState Evaluate() {
        Array.Clear(Colliders, 0, 3);
        Transform transform = (Transform) GetData(BTContextKey.Transform);
        int results = Physics.OverlapSphereNonAlloc(transform.position, itemInfo.detectionRange, Colliders, itemInfo.layer);

        if (results > 0) {
            List<Transform> enemies = new();

            foreach (Collider collider in Colliders) {
                if (collider != null) {
                    enemies.Add(collider.transform);
                }
            }

            parent.SetData(BTContextKey.Enemies, enemies);

            state = NodeState.SUCCESS;
            return state;
        } else {
            ClearData(BTContextKey.Enemies);
            state = NodeState.FAILURE;
            return state;
        }
    }
}