﻿/*
 * MIT License
 *
 * Copyright (c) 2018 Clark Yang
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Object = UnityEngine.Object;

namespace Framework
{
    public class Executors
    {

        private static readonly object syncLock = new object();
        private static bool disposed = false;
        private static MainThreadExecutor executor;
        private static int mainThreadId;

        static void Destroy()
        {
            disposed = true;
        }

        static Executors()
        {
            Create();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeCreate()
        {
            Create();
        }

        private static void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException("Executors");
        }
        
        public static void Create(bool dontDestroy = true, bool useFixedUpdate = true)
        {
            lock (syncLock)
            {
                try
                {
                    if (executor != null)
                        return;
                    mainThreadId = Environment.CurrentManagedThreadId;
                    executor = CreateMainThreadExecutor(dontDestroy, useFixedUpdate);
                    
                }
                catch (Exception e)
                {
                    Log.Error($"Start Executors failure.Exception:{e}");
                }
            }
        }
        
        private static MainThreadExecutor CreateMainThreadExecutor(bool dontDestroy, bool useFixedUpdate)
        {
            GameObject go = new GameObject("MainThreadExecutor");
            var createMainThreadExecutor = go.AddComponent<MainThreadExecutor>();
            go.hideFlags = HideFlags.HideAndDontSave;
            if (dontDestroy)
                Object.DontDestroyOnLoad(go);

            createMainThreadExecutor.useFixedUpdate = useFixedUpdate;
            return createMainThreadExecutor;
        }

        public static bool UseFixedUpdate
        {
            get => executor.useFixedUpdate;
            set => executor.useFixedUpdate = value;
        }

        public static bool IsMainThread => Environment.CurrentManagedThreadId == mainThreadId;

        public static void RunOnMainThread(Action action, bool waitForExecution = false)
        {
            if (disposed)
                return;

            if (waitForExecution)
            {
                AsyncResult result = AsyncResult.Create();
                RunOnMainThread(action, result);
                result.Synchronized().WaitForResult();
                return;
            }

            if (IsMainThread)
            {
                action();
                return;
            }

            executor.Execute(action);
        }

        public static TResult RunOnMainThread<TResult>(Func<TResult> func)
        {
            if (disposed)
                return default;

            AsyncResult<TResult> result = AsyncResult<TResult>.Create();
            RunOnMainThread(func, result);
            return result.Synchronized().WaitForResult();
        }

        public static void RunOnMainThread(Action action, IPromise promise)
        {
            try
            {
                CheckDisposed();

                if (IsMainThread)
                {
                    action();
                    promise.SetResult();
                    return;
                }

                executor.Execute(() =>
                {
                    try
                    {
                        action();
                        promise.SetResult();
                    }
                    catch (Exception e)
                    {
                        promise.SetException(e);
                    }
                });
            }
            catch (Exception e)
            {
                promise.SetException(e);
            }
        }

        public static void RunOnMainThread<TResult>(Func<TResult> func, IPromise<TResult> promise)
        {
            try
            {
                CheckDisposed();

                if (IsMainThread)
                {
                    promise.SetResult(func());
                    return;
                }

                executor.Execute(() =>
                {
                    try
                    {
                        promise.SetResult(func());
                    }
                    catch (Exception e)
                    {
                        promise.SetException(e);
                    }
                });
            }
            catch (Exception e)
            {
                promise.SetException(e);
            }
        }

        public static object WaitWhile(Func<bool> predicate)
        {
            if (executor != null && IsMainThread)
                return new WaitWhile(predicate);

            throw new NotSupportedException("The function must execute on main thread.");
        }

        protected static InterceptableEnumerator WrapEnumerator(IEnumerator routine, IPromise promise)
        {
            InterceptableEnumerator enumerator = routine is InterceptableEnumerator
                ? (InterceptableEnumerator) routine
                : new InterceptableEnumerator(routine);
            if (promise != null)
            {
                enumerator.RegisterConditionBlock(() => !(promise.IsCancellationRequested));
                enumerator.RegisterCatchBlock(e =>
                {
                    promise.SetException(e);

                    Log.Error(e);
                });
                enumerator.RegisterFinallyBlock(() =>
                {
                    if (!promise.IsDone)
                    {
                        if (promise.GetType().IsSubclassOfGenericTypeDefinition(typeof(IPromise<>)))
                            promise.SetException(new Exception("No value given the Result"));
                        else
                            promise.SetResult();
                    }
                });
            }

            return enumerator;
        }

        public static void RunOnCoroutineNoReturn(IEnumerator routine)
        {
            if (disposed || executor == null)
                return;

            if (IsMainThread)
            {
                executor.StartCoroutine(routine);
                return;
            }

            executor.Execute(routine);
        }

        public static Coroutine RunOnCoroutineReturn(IEnumerator routine)
        {
            if (disposed || executor == null)
                return null;

            if (IsMainThread)
            {
                return executor.StartCoroutine(routine);
            }

            AsyncResult<Coroutine> result = AsyncResult<Coroutine>.Create();
            executor.Execute(() =>
            {
                try
                {
                    Coroutine coroutine = executor.StartCoroutine(routine);
                    result.SetResult(coroutine);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });

            return result.Synchronized().WaitForResult();
        }

        public static void StopCoroutine(Coroutine routine)
        {
            if (disposed || executor == null)
                return;

            if (IsMainThread)
            {
                executor.StopCoroutine(routine);
                return;
            }

            executor.Stop(routine);
        }

        internal static void DoRunOnCoroutine(IEnumerator routine, ICoroutinePromise promise)
        {
            if (disposed)
            {
                promise.SetException(new ObjectDisposedException("Executors"));
                return;
            }

            if (executor == null)
            {
                promise.SetException(new ArgumentNullException("executor"));
                return;
            }

            if (IsMainThread)
            {
                Coroutine coroutine = executor.StartCoroutine(WrapEnumerator(routine, promise));
                promise.AddCoroutine(coroutine);
                return;
            }

            executor.Execute(() =>
            {
                try
                {
                    Coroutine coroutine = executor.StartCoroutine(WrapEnumerator(routine, promise));
                    promise.AddCoroutine(coroutine);
                }
                catch (Exception e)
                {
                    promise.SetException(e);
                }
            });
        }

        public static IAsyncResult RunOnCoroutine(IEnumerator routine)
        {
            CoroutineResult result = CoroutineResult.Create();
            DoRunOnCoroutine(routine, result);
            return result;
        }

        public static IAsyncResult RunOnCoroutine(Func<IPromise, IEnumerator> func)
        {
            CoroutineResult result = CoroutineResult.Create();
            DoRunOnCoroutine(func(result), result);
            return result;
        }

        public static IAsyncResult<TResult> RunOnCoroutine<TResult>(Func<IPromise<TResult>, IEnumerator> func)
        {
            CoroutineResult<TResult> result = CoroutineResult<TResult>.Create();
            DoRunOnCoroutine(func(result), result);
            return result;
        }

        public static IProgressResult<TProgress> RunOnCoroutine<TProgress>(
            Func<IProgressPromise<TProgress>, IEnumerator> func)
        {
            CoroutineProgressResult<TProgress> result = CoroutineProgressResult<TProgress>.Create();
            DoRunOnCoroutine(func(result), result);
            return result;
        }

        public static IProgressResult<TProgress, TResult> RunOnCoroutine<TProgress, TResult>(
            Func<IProgressPromise<TProgress, TResult>, IEnumerator> func)
        {
            CoroutineProgressResult<TProgress, TResult> result = CoroutineProgressResult<TProgress, TResult>.Create();
            DoRunOnCoroutine(func(result), result);
            return result;
        }

        public static void RunOnCoroutine(IEnumerator routine, IPromise promise)
        {
            if (disposed)
            {
                promise.SetException(new ObjectDisposedException("Executors"));
                return;
            }

            if (executor == null)
            {
                promise.SetException(new ArgumentNullException(nameof(promise)));
                return;
            }

            if (IsMainThread)
            {
                executor.StartCoroutine(WrapEnumerator(routine, promise));
                return;
            }

            executor.Execute(WrapEnumerator(routine, promise));
        }


        private static void DoRunAsync(Action action)
        {
#if UNITY_WEBGL
            throw new NotSupportedException("Multithreading is not supported on the WebGL platform.");
#else
            ThreadPool.QueueUserWorkItem((state) => { action(); });
#endif
        }

        public static IAsyncResult RunAsync(Action action)
        {
            AsyncResult result = AsyncResult.Create();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    action();
                    result.SetResult();
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return result;
        }

        public static IAsyncResult<TResult> RunAsync<TResult>(Func<TResult> func)
        {
            AsyncResult<TResult> result = AsyncResult<TResult>.Create();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    TResult tr = func();
                    result.SetResult(tr);
                }
                catch (Exception e)
                {
                    result.SetException(e);
                }
            });
            return result;
        }

        public static IAsyncResult RunAsync(Action<IPromise> action)
        {
            AsyncResult result = AsyncResult.Create();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    action(result);
                    if (!result.IsDone)
                        result.SetResult();
                }
                catch (Exception e)
                {
                    if (!result.IsDone)
                        result.SetException(e);
                }
            });
            return result;
        }

        public static IProgressResult<TProgress> RunAsync<TProgress>(Action<IProgressPromise<TProgress>> action)
        {
            ProgressResult<TProgress> result = ProgressResult<TProgress>.Create();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    action(result);
                    if (!result.IsDone)
                        result.SetResult(null);
                }
                catch (Exception e)
                {
                    if (!result.IsDone)
                        result.SetException(e);
                }
            });
            return result;
        }

        public static IAsyncResult<TResult> RunAsync<TResult>(Action<IPromise<TResult>> action)
        {
            AsyncResult<TResult> result = AsyncResult<TResult>.Create();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    action(result);
                    if (!result.IsDone)
                        result.SetResult(null);
                }
                catch (Exception e)
                {
                    if (!result.IsDone)
                        result.SetException(e);
                }
            });
            return result;
        }

        public static IProgressResult<TProgress, TResult> RunAsync<TProgress, TResult>(
            Action<IProgressPromise<TProgress, TResult>> action)
        {
            ProgressResult<TProgress, TResult> result = ProgressResult<TProgress, TResult>.Create();
            DoRunAsync(() =>
            {
                try
                {
                    CheckDisposed();
                    action(result);
                    if (!result.IsDone)
                        result.SetResult(null);
                }
                catch (Exception e)
                {
                    if (!result.IsDone)
                        result.SetException(e);
                }
            });
            return result;
        }

        class MainThreadExecutor : MonoBehaviour
        {
            public bool useFixedUpdate = false;
            private readonly List<object> _pendingQueue = new List<object>();
            private readonly List<object> _stopingQueue = new List<object>();

            private readonly List<object> _runningQueue = new List<object>();
            private readonly List<object> _stopingTempQueue = new List<object>();


            void OnApplicationQuit()
            {
                this.StopAllCoroutines();
                Executors.Destroy();
                if (this.gameObject != null)
                {
                    Destroy(this.gameObject);
                }
            }

            void Update()
            {
                if (useFixedUpdate)
                    return;

                if (_pendingQueue.Count <= 0 && _stopingQueue.Count <= 0)
                    return;

                this.DoStopingQueue();

                this.DoPendingQueue();

            }

            void FixedUpdate()
            {
                if (!useFixedUpdate)
                    return;

                if (_pendingQueue.Count <= 0 && _stopingQueue.Count <= 0)
                    return;

                this.DoStopingQueue();

                this.DoPendingQueue();
            }

            protected void DoStopingQueue()
            {
                lock (_stopingQueue)
                {
                    if (_stopingQueue.Count <= 0)
                        return;

                    _stopingTempQueue.Clear();
                    _stopingTempQueue.AddRange(_stopingQueue);
                    _stopingQueue.Clear();
                }

                for (int i = 0; i < _stopingTempQueue.Count; i++)
                {
                    try
                    {
                        object task = _stopingTempQueue[i];
                        if (task is IEnumerator enumerator)
                        {
                            this.StopCoroutine(enumerator);
                            continue;
                        }

                        if (task is Coroutine coroutine)
                        {
                            this.StopCoroutine(coroutine);
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Warning($"Task stop exception! error:{e}");
                    }
                }

                _stopingTempQueue.Clear();
            }

            protected void DoPendingQueue()
            {
                lock (_pendingQueue)
                {
                    if (_pendingQueue.Count <= 0)
                        return;

                    _runningQueue.Clear();
                    _runningQueue.AddRange(_pendingQueue);
                    _pendingQueue.Clear();
                }

                float startTime = Time.realtimeSinceStartup;
                for (int i = 0; i < _runningQueue.Count; i++)
                {
                    try
                    {
                        object task = _runningQueue[i];
                        if (task is Action action)
                        {
                            action();
                            continue;
                        }

                        if (task is IEnumerator enumerator)
                        {
                            this.StartCoroutine(enumerator);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Warning($"Task execution exception! error:{e}");
                    }
                }

                _runningQueue.Clear();

                float time = Time.realtimeSinceStartup - startTime;
                if (time > 0.15f)
                    Log.Msg(
                        $"The running time of tasks in the main thread executor is too long.these tasks take {(int) (time * 1000)} milliseconds.");
            }

            public void Execute(Action action)
            {
                if (action == null)
                    return;

                lock (_pendingQueue)
                {
                    _pendingQueue.Add(action);
                }
            }

            public void Execute(IEnumerator routine)
            {
                if (routine == null)
                    return;

                lock (_pendingQueue)
                {
                    _pendingQueue.Add(routine);
                }
            }

            /// <summary>
            /// Stop Coroutine
            /// </summary>
            /// <param name="routine"></param>
            public void Stop(IEnumerator routine)
            {
                if (routine == null)
                    return;

                lock (_pendingQueue)
                {
                    if (_pendingQueue.Contains(routine))
                    {
                        _pendingQueue.Remove(routine);
                        return;
                    }
                }

                lock (_stopingQueue)
                {
                    _stopingQueue.Add(routine);
                }
            }

            /// <summary>
            /// Stop Coroutine
            /// </summary>
            /// <param name="routine"></param>
            public void Stop(Coroutine routine)
            {
                if (routine == null)
                    return;

                lock (_stopingQueue)
                {
                    _stopingQueue.Add(routine);
                }
            }
        }
    }
}