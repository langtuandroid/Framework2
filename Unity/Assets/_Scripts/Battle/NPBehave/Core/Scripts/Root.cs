using UnityEngine;

namespace NPBehave
{
    public class Root : Decorator
    {
        public Node MainNode { get; }

        public bool IsLoop { get; }

        private Blackboard blackboard;

        public event System.Action OnFinish;

        public override Blackboard Blackboard
        {
            get { return blackboard; }
        }


        private Clock clock;

        public override Clock Clock
        {
            get { return clock; }
        }

        public Root(Node mainNode, Clock clock, bool isLoop) : base("Root", mainNode)
        {
            this.IsLoop = isLoop;
            this.MainNode = mainNode;
            //    m_MainNodeStartActionCache = this.mainNode.Start;
            this.clock = clock;
            this.blackboard = new Blackboard(this.clock);
            this.SetRoot(this);
        }

        public Root(Blackboard blackboard, Clock clock, Node mainNode) : base("Root", mainNode)
        {
            this.blackboard = blackboard;
            this.MainNode = mainNode;
            this.clock = clock;
            this.SetRoot(this);
        }

        public override void SetRoot(Root rootNode)
        {
            Debug.Assert(this == rootNode);
            base.SetRoot(rootNode);
            this.MainNode.SetRoot(rootNode);
        }

        override protected void DoStart()
        {
            this.blackboard.Enable();
            this.MainNode.Start();
        }

        override protected void DoCancel()
        {
            if (this.MainNode.IsActive)
            {
                this.MainNode.Cancel();
            }
            else
            {
                this.clock.RemoveTimer(this.MainNode.Start);
            }
        }

        override protected void DoChildStopped(Node node, bool success)
        {
            OnFinish?.Invoke();
            if (IsLoop && !IsStopRequested)
            {
                // wait one tick, to prevent endless recursions
                this.clock.AddTimer(0, 0, this.MainNode.Start);
            }
            else
            {
                this.blackboard.Disable();
                Stopped(success);
            }
        }

        public void CancelWithoutReturnResult()
        {
            //Assert.AreEqual(this.currentState, State.ACTIVE, "can only stop active nodes, tried to stop " + this.Name + "! PATH: " + GetPath());
            Debug.Assert(this.currentState == State.ACTIVE,
                $"can only stop active nodes, tried to stop  PATH: {GetPath()}");
            this.currentState = State.STOP_REQUESTED;
            DoCancel();
        }
    }
}