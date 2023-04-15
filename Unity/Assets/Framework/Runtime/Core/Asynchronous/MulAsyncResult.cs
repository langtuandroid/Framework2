using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Framework
{
    public interface IMulProgress
    {
        float Current { get; }
        float Total { get; }
    }
    
    public class MulAsyncResult : ProgressResult<float>
    {
        private RecyclableList<IAsyncResult> _allProgress;
        public override bool IsDone => _allProgress.Count <= 0 || base.IsDone;

        private MulAsyncResult()
        {
        }

        public static MulAsyncResult Create(bool cancelable = true, bool isFromPool = true, params IAsyncResult[] allProgress)
        {
            var result = isFromPool ? ReferencePool.Allocate<MulAsyncResult>() : new MulAsyncResult();
            result._allProgress = RecyclableList<IAsyncResult>.Create();
            result.Cancelable = cancelable;
            result.IsFromPool = isFromPool;
            result.AddAsyncResult(allProgress);
            return result;
        }

        public void AddAsyncResult(IAsyncResult progressResult)
        {
            if (progressResult == null) return;
            _allProgress.Add(progressResult);
            SetSubProgressCb(progressResult);
        }

        public void AddAsyncResult(IEnumerable<IAsyncResult> progressResults)
        {
            foreach (var progressResult in progressResults)
            {
                AddAsyncResult(progressResult);
            }
        }

        private void SetSubProgressCb(IAsyncResult progressResult)
        {
            if (progressResult.IsDone) return;
            progressResult.Callbackable().OnCallback(f => RaiseOnProgressCallback(0));
        }

        private bool CheckAllFinish()
        {
            for (var index = 0; index < _allProgress.Count; index++)
            {
                var progressResult = _allProgress[index];
                if (!progressResult.IsDone) return false;
            }

            return true;
        }
        
        protected virtual async void RaiseOnProgressCallback(float progress)
        {
            UpdateProgress();
            //延迟一帧 否则会比子任务提前完成
            if (CheckAllFinish())
            {
                await TimerComponent.Instance.WaitFrameAsync();
                SetResult();
            }
        }

        private void UpdateProgress()
        {
            float totalProgress = 0;
            for (var index = 0; index < _allProgress.Count; index++)
            {
                var progressResult = _allProgress[index];
                if (progressResult.IsDone)
                {
                    totalProgress += 1;
                }
            }

            Progress = totalProgress / _allProgress.Count;
        }

        public override void Clear()
        {
            base.Clear();
            foreach (var asyncResult in _allProgress)
            {
                ReferencePool.Free(asyncResult);
            }
            ReferencePool.Free(_allProgress);
            _allProgress = null;
        }
    }
    
    public class MulProgressResult : ProgressResult<float>
    {
        private RecyclableList<IProgressResult<float>> _allProgress;
        public override bool IsDone => _allProgress.Count <= 0 || base.IsDone;

        private MulProgressResult()
        {
        }

        public static MulProgressResult Create(bool cancelable = true,bool isFromPool = true, params IProgressResult<float>[] allProgress)
        {
            var result = isFromPool ? ReferencePool.Allocate<MulProgressResult>() : new MulProgressResult();
            result._allProgress = RecyclableList<IProgressResult<float>>.Create();
            result.Cancelable = cancelable;
            result.IsFromPool = isFromPool;
            result.AddAsyncResult(allProgress);
            return result;
        }

        public void AddAsyncResult(IProgressResult<float> progressResult)
        {
            if (progressResult == null) return;
            _allProgress.Add(progressResult);
            SetSubProgressCb(progressResult);
        }

        public void AddAsyncResult(IEnumerable<IProgressResult<float>> progressResults)
        {
            foreach (var progressResult in progressResults)
            {
                AddAsyncResult(progressResult);
            }
        }

        private void SetSubProgressCb(IProgressResult<float> progressResult)
        {
            if (progressResult.IsDone) return;
            progressResult.Callbackable().OnProgressCallback((_ => RaiseOnProgressCallback(0)));
            progressResult.Callbackable().OnCallback(_ =>
            {
                if (CheckAllFinish())
                {
                    RaiseFinish();
                }
            });
        }

        protected override void RaiseOnProgressCallback(float progress)
        {
            UpdateProgress();
            base.RaiseOnProgressCallback(Progress);
        }
        
        private bool CheckAllFinish()
        {
            foreach (var progressResult in _allProgress)
            {
                if(!progressResult.IsDone) return false;
            }
            return true;
        }

        private async void RaiseFinish()
        {
            StringBuilder sb = null;
            foreach (var progressResult in _allProgress)
            {
                if (progressResult.Exception == null) continue;
                if (sb == null) sb = new StringBuilder();
                sb.AppendLine(progressResult.Exception.ToString());
            }

            //延迟一帧 否则会比子任务提前完成
            await TimerComponent.Instance.WaitFrameAsync();
            if (sb != null)
            {
                SetException(sb.ToString());
            }
            else
            {
                SetResult();
            }
        }

        private void UpdateProgress()
        {
            float totalProgress = 0;
            foreach (var progressResult in _allProgress)
            {
                if (progressResult.IsDone)
                {
                    totalProgress += 1;
                }
                else
                {
                    totalProgress += progressResult.Progress;
                }
            }
            Progress = totalProgress / _allProgress.Count;
        }

        public override void Clear()
        {
            base.Clear();
            foreach (var asyncResult in _allProgress)
            {
                ReferencePool.Free(asyncResult);
            }
            ReferencePool.Free(_allProgress);
            _allProgress = null; 
        }
    }
}