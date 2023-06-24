using System;

namespace NPBehave
{
    public class WaitUntil : Task
    {
        public WaitUntil(Func<bool> checkFinish) : base("waitUntil")
        {
            this.checkFinish = checkFinish;
        }
        
        private Func<bool> checkFinish = null;

        protected override void DoStart()
        {
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