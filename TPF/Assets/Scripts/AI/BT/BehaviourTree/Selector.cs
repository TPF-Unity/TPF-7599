using System.Collections.Generic;

namespace BehaviourTree {
    public class Selector : Node {
        // OR node
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        // Si algun nodo es running, el Selector es running y se devuelve el running
        // Si algun nodo es success, el Selector es success y se devuelve el success
        // Si ningun nodo esta en running o success, el Selector es failure y se devuelve el failure
        public override NodeState Evaluate() {
            foreach (Node child in children) {
                switch (child.Evaluate()) {
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.FAILURE:
                        break;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}