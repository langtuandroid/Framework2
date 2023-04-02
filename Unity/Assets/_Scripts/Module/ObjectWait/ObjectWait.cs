using System;
using System.Collections.Generic;
using System.Linq;
using Framework;

namespace Framework
{
    public static class WaitTypeError
    {
        public const int Success = 0;
        public const int Destroy = 1;
        public const int Cancel = 2;
        public const int Timeout = 3;
    }

    public interface IWaitType
    {
        int Error { get; set; }
    }


    public class ObjectWait : Entity, IAwakeSystem, IDestroySystem
    {
        public Dictionary<Type, object> tcss = new Dictionary<Type, object>();

        public void Awake(Entity o)
        {
            tcss.Clear();
        }

        private interface IDestroyRun
        {
            void SetResult();
        }

        private class ResultCallback<K> : IDestroyRun where K : struct, IWaitType
        {
            private ETTask<K> tcs;

            public ResultCallback()
            {
                this.tcs = ETTask<K>.Create(true);
            }

            public bool IsDisposed
            {
                get { return this.tcs == null; }
            }

            public ETTask<K> Task => this.tcs;

            public void SetResult(K k)
            {
                var t = tcs;
                this.tcs = null;
                t.SetResult(k);
            }

            public void SetResult()
            {
                var t = tcs;
                this.tcs = null;
                t.SetResult(new K() { Error = WaitTypeError.Destroy });
            }
        }

        public async ETTask<T> Wait<T>(ETCancellationToken cancellationToken = null)
            where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T>();
            Type type = typeof(T);
            tcss.Add(type, tcs);

            void CancelAction()
            {
                Notify(new T() { Error = WaitTypeError.Cancel });
            }

            T ret;
            try
            {
                cancellationToken?.Add(CancelAction);
                ret = await tcs.Task;
            }
            finally
            {
                cancellationToken?.Remove(CancelAction);
            }

            return ret;
        }

        public async ETTask<T> Wait<T>(int timeout,
            ETCancellationToken cancellationToken = null) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T>();

            async ETTask WaitTimeout()
            {
                await TimerComponent.Instance.WaitAsync(timeout, cancellationToken);
                if (cancellationToken.IsCancel())
                {
                    return;
                }

                if (tcs.IsDisposed)
                {
                    return;
                }

                Notify(new T() { Error = WaitTypeError.Timeout });
            }

            WaitTimeout().Coroutine();

            tcss.Add(typeof(T), tcs);

            void CancelAction()
            {
                Notify(new T() { Error = WaitTypeError.Cancel });
            }

            T ret;
            try
            {
                cancellationToken?.Add(CancelAction);
                ret = await tcs.Task;
            }
            finally
            {
                cancellationToken?.Remove(CancelAction);
            }

            return ret;
        }

        public void Notify<T>(T obj) where T : struct, IWaitType
        {
            Type type = typeof(T);
            if (!tcss.TryGetValue(type, out object tcs))
            {
                return;
            }

            tcss.Remove(type);
            ((ResultCallback<T>)tcs).SetResult(obj);
        }


        public void OnDestroy(Entity o)
        {
            foreach (object v in tcss.Values.ToArray())
            {
                ((IDestroyRun)v).SetResult();
            }
        }
    }
}