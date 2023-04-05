﻿namespace NPBehave
{
    public class Succeeder : Decorator
    {
        public Succeeder(Node decoratee) : base("Succeeder", decoratee)
        {
        }

        protected override void DoStart()
        {
            Decoratee.Start();
        }

        override protected void DoCancel()
        {
            Decoratee.Cancel();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            Stopped(true);
        }
    }
}