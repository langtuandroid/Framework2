using System;
using System.Collections.Generic;

namespace Framework
{

    public class RecyclableDic<TKey,TValue> : Dictionary<TKey, TValue> , IDisposable
    {
        private bool disposed = false;

        public static  RecyclableDic<TKey,TValue> Create()
        {
            var result = ObjectPool.Instance.Fetch(typeof( RecyclableDic<TKey,TValue>)) as  RecyclableDic<TKey,TValue>;
            result.disposed = false;
            return result;
        }
        
        public void Dispose()
        {
            if(disposed) return;
            disposed = true;
            this.Clear();
            ObjectPool.Instance.Recycle(this);
        } 
    }
}