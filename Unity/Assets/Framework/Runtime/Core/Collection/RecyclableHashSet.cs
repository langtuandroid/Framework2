using System;
using System.Collections.Generic;

namespace Framework
{
    public class RecyclableHashSet<T> : HashSet<T>, IDisposable
    {
        private bool disposed = false;
        
        public static RecyclableHashSet<T> Create()
        {
            var result = ObjectPool.Instance.Fetch(typeof(RecyclableHashSet<T>)) as RecyclableHashSet<T>;
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