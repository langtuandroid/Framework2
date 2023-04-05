using System;

namespace NPBehave
{
    public class Random : Decorator
    {
        private float probability;
        private System.Random random;

        public Random(float probability, Node decoratee) : base("Random", decoratee)
        {
            this.probability = probability;
            random = new System.Random();
        }

        protected override void DoStart()
        {
            if (random.NextDouble() <= this.probability)
            {
                Decoratee.Start();
            }
            else
            {
                Stopped(false);
            }
        }

        override protected void DoCancel()
        {
            Decoratee.Cancel();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            Stopped(result);
        }
    }
}