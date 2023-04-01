using UnityEngine;

namespace NPBehave
{
    public class Root : Decorator
    {
        private Node mainNode;

        //private Node inProgressNode;

        private Blackboard blackboard;

        public override Blackboard Blackboard
        {
            get { return blackboard; }
        }


        private Clock clock;

        public override Clock Clock
        {
            get { return clock; }
        }

        public Root(Node mainNode, Clock clock) : base("Root", mainNode)
        {
            this.mainNode = mainNode;
            //    m_MainNodeStartActionCache = this.mainNode.Start;
            this.clock = clock;
            this.blackboard = new Blackboard(this.clock);
            this.SetRoot(this);
        }

        public Root(Blackboard blackboard, Clock clock, Node mainNode) : base("Root", mainNode)
        {
            this.blackboard = blackboard;
            this.mainNode = mainNode;
            this.clock = clock;
            this.SetRoot(this);
        }

        public override void SetRoot(Root rootNode)
        {
            Debug.Assert(this == rootNode);
            base.SetRoot(rootNode);
            this.mainNode.SetRoot(rootNode);
        }


        override protected void DoStart()
        {
            this.blackboard.Enable();
            this.mainNode.Start();
        }

        override protected void DoStop()
        {
            if (this.mainNode.IsActive)
            {
                this.mainNode.Stop();
            }
            else
            {
                this.clock.RemoveTimer(this.mainNode.Start);
            }
        }


        override protected void DoChildStopped(Node node, bool success)
        {
            if (!IsStopRequested)
            {
                // wait one tick, to prevent endless recursions
                this.clock.AddTimer(0, 0, this.mainNode.Start);
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
            DoStop();
        }
    }
}