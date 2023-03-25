using System;
using System.Collections.Generic;

namespace Framework
{
    public class RecyclableList<T> : List<T>, IDisposable
    {
        private bool disposed = false;

        public static RecyclableList<T> Create()
        {
            var result = ObjectPool.Instance.Fetch(typeof(RecyclableList<T>)) as RecyclableList<T>;
            result.disposed = false;
            return result;
        }

        public static RecyclableList<T> Create(IEnumerable<T> collection)
        {
            var result = Create();
            result.AddRange(collection);
            return result;
        }
        
        public override string ToString()
        {
            return $"[ {string.Join(" | ", this)} ]";
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