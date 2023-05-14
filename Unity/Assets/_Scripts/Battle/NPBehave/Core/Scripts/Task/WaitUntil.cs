using System;

namespace NPBehave
{
    public class WaitUntil : Task
    {
        public WaitUntil(out Action<bool> finishCb) : base("waitUntil")
        {
            finishCb = Finish;
        }

        private void Finish(bool isSuccess)
        {
        } 
    }
}