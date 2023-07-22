﻿using UnityEngine;

namespace NPBehave
{
    public class Action : Task
    {
        public enum Result
        {
            SUCCESS,
            FAILED,
            BLOCKED,
            PROGRESS
        }

        public enum Request
        {
            START,
            UPDATE,
            CANCEL,
        }

        private System.Func<bool> singleFrameFunc = null;
        private System.Func<bool, Result> multiFrameFunc = null;
        private System.Func<Request, Result> multiFrameFunc2 = null;
        private System.Action action = null;
        private bool bWasBlocked = false;

        public Action(System.Action action, string debugName) : base(debugName)
        {
            this.action = action;
        }

        public Action(System.Func<bool, Result> multiframeFunc, string debugName) : base(debugName)
        {
            this.multiFrameFunc = multiframeFunc;
        }

        public Action(System.Func<Request, Result> multiframeFunc2, string debugName) : base(debugName)
        {
            this.multiFrameFunc2 = multiframeFunc2;
        }


        public Action(System.Func<bool> singleFrameFunc, string debugName) : base(debugName)
        {
            this.singleFrameFunc = singleFrameFunc;
        }

        protected override void DoStart()
        {
            if (this.action != null)
            {
                this.action.Invoke();
                this.Stopped(true);
            }
            else if (this.multiFrameFunc != null)
            {
                Result result = this.multiFrameFunc.Invoke(false);
                if (result == Result.PROGRESS)
                {
                    this.RootNode.Clock.AddUpdateObserver(OnUpdateFunc);
                }
                else if (result == Result.BLOCKED)
                {
                    this.bWasBlocked = true;
                    this.RootNode.Clock.AddUpdateObserver(OnUpdateFunc);
                }
                else
                {
                    this.Stopped(result == Result.SUCCESS);
                }
            }
            else if (this.multiFrameFunc2 != null)
            {
                Result result = this.multiFrameFunc2.Invoke(Request.START);
                if (result == Result.PROGRESS)
                {
                    this.RootNode.Clock.AddUpdateObserver(OnUpdateFunc2);
                }
                else if (result == Result.BLOCKED)
                {
                    this.bWasBlocked = true;
                    this.RootNode.Clock.AddUpdateObserver(OnUpdateFunc2);
                }
                else
                {
                    this.Stopped(result == Result.SUCCESS);
                }
            }
            else if (this.singleFrameFunc != null)
            {
                this.Stopped(this.singleFrameFunc.Invoke());
            }
        }

        private void OnUpdateFunc()
        {
            Result result = this.multiFrameFunc.Invoke(false);
            if (result != Result.PROGRESS && result != Result.BLOCKED)
            {
                this.RootNode.Clock.RemoveUpdateObserver(OnUpdateFunc);
                this.Stopped(result == Result.SUCCESS);
            }
        }

        private void OnUpdateFunc2()
        {
            Result result = this.multiFrameFunc2.Invoke(bWasBlocked ? Request.START : Request.UPDATE);

            if (result == Result.BLOCKED)
            {
                bWasBlocked = true;
            }
            else if (result == Result.PROGRESS)
            {
                bWasBlocked = false;
            }
            else
            {
                this.RootNode.Clock.RemoveUpdateObserver(OnUpdateFunc2);
                this.Stopped(result == Result.SUCCESS);
            }
        }

        protected override void DoCancel()
        {
            if (this.multiFrameFunc != null)
            {
                Result result = this.multiFrameFunc.Invoke(true);
                Debug.Assert(result != Result.PROGRESS,
                    "The Task has to return Result.SUCCESS, Result.FAILED/BLOCKED after beeing cancelled!");
                this.RootNode.Clock.RemoveUpdateObserver(OnUpdateFunc);
                this.Stopped(result == Result.SUCCESS);
            }
            else if (this.multiFrameFunc2 != null)
            {
                Result result = this.multiFrameFunc2.Invoke(Request.CANCEL);
                Debug.Assert(result != Result.PROGRESS,
                    "The Task has to return Result.SUCCESS or Result.FAILED/BLOCKED after beeing cancelled!");
                this.RootNode.Clock.RemoveUpdateObserver(OnUpdateFunc2);
                this.Stopped(result == Result.SUCCESS);
            }
            else
            {
                Debug.Assert(false, "DoStop called for a single frame action on " + this);
            }
        }
    }
}