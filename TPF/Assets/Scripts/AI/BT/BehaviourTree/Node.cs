using System.Collections.Generic;

namespace BehaviourTree {
    public enum NodeState {
        RUNNING,
        SUCCESS,
        FAILURE,
    }

    public class Node {
        protected NodeState state;
        protected List<Node> children;
        public Node parent;
        public string name;

        private readonly Dictionary<BTContextKey, object> context = new();

        public Node() {
            this.parent = null;
            this.children = new List<Node>();
        }

        public Node(List<Node> children) : this(children, "name") {}

        public Node(List<Node> children, string name) {
            this.name = name;
            this.parent = null;
            this.children = new();
            
            if (children != null) {
                foreach (Node child in children) {
                    AttachNode(child);
                }
            }
        }

        public void AttachNode(Node node) {
            children.Add(node);
            node.parent = this;
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(BTContextKey key, object value) {
            context[key] = value;
        }

        public object GetData(BTContextKey key) {
            context.TryGetValue(key, out object data);
            if (data == null) {
                if (parent != null) {
                    data = parent.GetData(key);
                } else {
                    data = null;
                }
            }

            return data;
        }

        public bool ClearData(BTContextKey key) {
            context.TryGetValue(key, out object data);

            if (context.ContainsKey(key)) {
                context.Remove(key);
                return true;
            } else {
                if (parent != null) {
                    return parent.ClearData(key);
                } else {
                    return false;
                }
            }
        }

        public string Show(string prefix = "") {
            string result = "";

            result += prefix + name + "\n";

            foreach (Node child in children) {
                result += child.Show(prefix + "\t");
            }

            return result;
        }
    }
}