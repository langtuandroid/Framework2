using System;

namespace Framework
{

    public abstract class ConfigSingleton<T> : ISingleton where T : ConfigSingleton<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get { return instance ??= ConfigComponent.Instance.LoadOneConfig(typeof(T)) as T; }
        }

        void ISingleton.Register()
        {
            if (instance != null)
            {
                throw new Exception($"singleton register twice! {typeof(T).Name}");
            }

            instance = (T)this;
        }

        void ISingleton.Destroy()
        {
            T t = instance;
            instance = null;
            t.Dispose();
        }

        bool ISingleton.IsDisposed()
        {
            throw new NotImplementedException();
        }

        public void AfterEndInit()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}