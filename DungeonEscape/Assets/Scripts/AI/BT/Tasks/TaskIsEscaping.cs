using System;
using AI.GOAP.Config;
using BehaviourTree;
using UnityEngine;

public class TaskIsEscaping : Node {
    private float cooldown = 5f;

    public TaskIsEscaping() {}

    public override NodeState Evaluate() {
        if (GetData(BTContextKey.Escaping) != null && (bool) GetData(BTContextKey.Escaping)) {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0f) {
                Node root = (Node) GetData(BTContextKey.Root);
                root.SetData(BTContextKey.Escaping, false);
                cooldown = 5f;

                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.SUCCESS;
            return state;
        } else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}