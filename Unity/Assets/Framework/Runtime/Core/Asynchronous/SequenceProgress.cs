using System;
using System.Collections.Generic;

namespace Framework
{
    public class SequenceProgress : ProgressResult<float>
    {
        private RecyclableList<Func<IProgressResult<float>>> progressQueue;
        private int index = 0;
        private IProgressResult<float> currentProgress;

        public override bool IsDone => currentProgress == null || base.IsDone;

        private SequenceProgress()
        {
        }

        public static SequenceProgress Create(bool cancelable = true, bool isFromPool = true, bool needDelayFreePool = false, params Func<IProgressResult<float>>[] allProgress)
        {
            var result = isFromPool ? ReferencePool.Allocate<SequenceProgress>() : new SequenceProgress();
            result.OnCreate(cancelable, isFromPool, needDelayFreePool);
            result.AddAsyncResult(allProgress);
            return result;
        }

        public void AddAsyncResult(Func<IProgressResult<float>> progressResult)
        {
            if(progressResult == null) return;
            progressQueue.Add(progressResult);
            if (currentProgress == null)
            {
                SetNextProgress();
            }
        }

        private void SetNextProgress()
        {
            if (index < progressQueue.Count)
            {
                currentProgress = progressQueue[index].Invoke();
                index++;
                SetSubProgressCb(currentProgress);
            }
            else
            {
                currentProgress = null;
                RaiseFinish();
            }
        }

        public void AddAsyncResult(IEnumerable<Func<IProgressResult<float>>> progressResults)
        {
            foreach (var progressResult in progressResults)
            {
                AddAsyncResult(progressResult);
            }
        }

        private void SetSubProgressCb(IProgressResult<float> progressResult)
        {
            progressResult.Callbackable().OnProgressCallback((_ => RaiseOnProgressCallback(0)));
            progressResult.Callbackable().OnCallback(_ =>
            {
                    SetNextProgress();
            });
        }

        protected override void RaiseOnProgressCallback(float progress)
        {
            UpdateProgress();
            base.RaiseOnProgressCallback(Progress);
        }

        private async void RaiseFinish()
        {
            await TimerComponent.Instance.WaitFrameAsync();
            SetResult();
        }

        private void UpdateProgress()
        {
            float totalProgress = index + currentProgress.Progress;
            // 1 是当前正在执行的progress
            Progress = totalProgress / progressQueue.Count;
        }

        public override void Clear()
        {
            base.Clear();
            ReferencePool.Free(progressQueue);
            index = 0;
        }
    }
}