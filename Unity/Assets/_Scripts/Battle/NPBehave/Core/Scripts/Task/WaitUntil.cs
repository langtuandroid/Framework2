using System;

namespace NPBehave
{
    public class WaitUntil : Task
    {
        public WaitUntil(System.Action onStart,Func<bool> checkFinish) : base("waitUntil")
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
            Clock.RemoveTimer(onTimer);
            this.Stopped(false);
        }

        private void onTimer()
        {
            if (checkFinish())
            {
                Clock.RemoveTimer(onTimer);
                this.Stopped(true);
            }
        }
    }
}