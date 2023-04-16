using System;
using System.Collections.Generic;

namespace UnityExtensions
{
    internal abstract class BaseObjectPool<T> where T : class
    {
        Stack<T> _objects;


        internal int count => _objects.Count;


        internal BaseObjectPool(int initialQuantity = 0)
        {
            _objects = new Stack<T>(initialQuantity > 16 ? initialQuantity : 16);
            AddObjects(initialQuantity);
        }


        protected abstract T CreateInstance();


        internal void AddObjects(int quantity)
        {
            while (quantity > 0)
            {
                _objects.Push(CreateInstance());
                quantity--;
            }
        }


        internal T Spawn()
        {
            return _objects.Count > 0 ? _objects.Pop() : CreateInstance();
        }


        internal void Despawn(T target)
        {
            _objects.Push(target);
        }


        internal TempObject GetTemp()
        {
            return new TempObject(this);
        }


        internal struct TempObject : IDisposable
        {
            internal T item { get; private set; }
            BaseObjectPool<T> _pool;

            internal TempObject(BaseObjectPool<T> objectPool)
            {
                item = objectPool.Spawn();
                _pool = objectPool;
            }

            public static implicit operator T(TempObject temp) => temp.item;

            void IDisposable.Dispose()
            {
                _pool.Despawn(item);
                item = null;
                _pool = null;
            }
        }

    } // class BaseObjectPool

    internal class ObjectPool<T> : BaseObjectPool<T> where T : class, new()
    {
        protected override T CreateInstance() => new T();

        internal ObjectPool(int initialQuantity = 0) : base(initialQuantity) { }

    } // class ObjectPool

} // namespace UnityExtensions