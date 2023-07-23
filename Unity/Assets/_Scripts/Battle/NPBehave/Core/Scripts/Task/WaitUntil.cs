using System;

namespace NPBehave
{
    public class WaitUntil : Task
    {
        public WaitUntil(System.Action onStart, Func<bool> checkFinish, string name = nameof(WaitUntil)) : base(name)
        {
            this.onStart = onStart;
            this.checkFinish = checkFinish;
        }
        
        private Func<bool> checkFinish = null;
        private System.Action onStart = null;

        protected override void DoStart()
        {
            onStart?.Invoke();
            Clock.AddUpdateObserver(onTimer);
        }

        protected override void DoCancel()
        {
            Clock.RemoveUpdateObserver(onTimer);
            this.Stopped(false);
        }

        private void onTimer()
        {
            if (checkFinish())
            {
                Clock.RemoveUpdateObserver(onTimer);
                this.Stopped(true);
            }
        }
    }
}