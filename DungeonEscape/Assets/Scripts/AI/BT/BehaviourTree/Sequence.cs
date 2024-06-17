using System.Collections.Generic;

namespace BehaviourTree {
    public class Sequence : Node {
        // AND node
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        // Si algun nodo no tiene state definido se asigna state success y se devuelve el success
        // Si algun nodo esta failure, el Sequence es failure y se devuelve el failure
        // Si ninguno es failure pero hay alguno running, el Sequence es running y se devuelve el running
        // Si no hay ninguno failure/running, el Sequence es success y se devuelve el success
        public override NodeState Evaluate() {
            bool anyRunning = false;

            foreach (Node child in children) {
                switch (child.Evaluate()) {
                    case NodeState.RUNNING:
                        anyRunning = true;
                        break;
                    case NodeState.SUCCESS:
                        break;
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}