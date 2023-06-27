using UnityEngine;

namespace NPBehave
{
    public abstract class Composite : Container
    {
        protected Node[] Children;

        public Composite(string name, Node[] children) : base(name)
        {
            this.Children = children;
            Debug.Assert(children.Length > 0,
                "Composite nodes (Selector, Sequence, Parallel) need at least one child!");

            foreach (Node node in Children)
            {
                node.SetParent(this);
            }
        }

        override public void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);

            foreach (Node node in Children)
            {
                node.SetRoot(rootNode);
            }
        }


#if UNITY_EDITOR
        public override Node[] DebugChildren
        {
            get { return this.Children; }
        }

        public Node DebugGetActiveChild()
        {
            foreach (Node node in DebugChildren)
            {
                if (node.CurrentState == Node.State.ACTIVE)
                {
                    return node;
                }
            }

            return null;
        }
#endif

        protected override void Stopped(bool success)
        {
            foreach (Node child in Children)
            {
                child.ParentCompositeStopped(this);
            }

            base.Stopped(success);
        }

        public override void ChangeChild(Node oldChild, Node newChild)
        {
            for (int i = 0; i < Children.Length; i++)
            {
                if (Children[i] == oldChild)
                {
                    Children[i] = newChild;
                    newChild.SetParent(this);
                    oldChild.SetParent(null);
                }
            }
        }
        
        public abstract void StopLowerPriorityChildrenForChild(Node child, bool immediateRestart);
    }
}