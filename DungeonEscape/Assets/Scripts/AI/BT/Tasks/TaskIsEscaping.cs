using System;
using AI.GOAP.Config;
using BehaviourTree;
using UnityEngine;

public class TaskIsEscaping : Node {
    private float cooldown = 3f;

    public TaskIsEscaping() {}

    public override NodeState Evaluate() {
        if (GetData(BTContextKey.Escaping) != null && (bool) GetData(BTContextKey.Escaping)) {
            cooldown -= Time.deltaTime;

            Node root = (Node) GetData(BTContextKey.Root);
            Vector3 escapeRoute = (Vector3) GetData(BTContextKey.EscapeRoute);
            Transform transform = (Transform) GetData(BTContextKey.Transform);

            if (cooldown <= 0f || Vector3.Distance(escapeRoute, transform.position) < 1f) {
                root.SetData(BTContextKey.Escaping, false);
                cooldown = 3f;

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