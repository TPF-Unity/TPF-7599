using System.Collections.Generic;

namespace BehaviourTree {
    public class Inverter : Node {
        // NOT node
        public Inverter() : base() { }
        public Inverter(Node child) : base(new List<Node>() { child }) { }

        // Invierte el resultado del nodo hijo
        public override NodeState Evaluate() {
            Node child = children[0];
            switch (child.Evaluate()) {
                case NodeState.RUNNING:
                    state = NodeState.RUNNING;
                    return state;
                case NodeState.SUCCESS:
                    state = NodeState.FAILURE;
                    return state;
                case NodeState.FAILURE:
                    state = NodeState.SUCCESS;
                    return state;
                default:
                    state = NodeState.RUNNING;
                    return state;
            }
        }
    }
}