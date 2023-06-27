namespace NPBehave
{
    public abstract class Decorator : Container
    {
        protected Node Decoratee;

        public Decorator(string name, Node decoratee) : base(name)
        {
            this.Decoratee = decoratee;
            this.Decoratee.SetParent(this);
        }

        override public void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);
            Decoratee.SetRoot(rootNode);
        }


#if UNITY_EDITOR
        public override Node[] DebugChildren
        {
            get { return new Node[] { Decoratee }; }
        }
#endif

        public override void ChangeChild(Node oldChild, Node newChild)
        {
            if(Decoratee != oldChild) return;
            Decoratee.SetParent(null);
            newChild.SetParent(this);
            Decoratee = newChild;
        }

        public override void ParentCompositeStopped(Composite composite)
        {
            base.ParentCompositeStopped(composite);
            Decoratee.ParentCompositeStopped(composite);
        }
    }
}