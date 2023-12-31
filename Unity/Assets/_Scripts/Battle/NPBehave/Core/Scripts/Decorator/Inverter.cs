﻿namespace NPBehave
{
    public class Inverter : Decorator
    {
        public Inverter(Node decoratee) : base("Inverter", decoratee)
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
            Stopped(!result);
        }
    }
}