using System;
using System.Collections.Generic;

namespace Framework
{
    public class NumericWatcherInfo
    {
        public SceneType SceneType { get; }
        public INumericWatcher INumericWatcher { get; }

        public NumericWatcherInfo(SceneType sceneType, INumericWatcher numericWatcher)
        {
            this.SceneType = sceneType;
            this.INumericWatcher = numericWatcher;
        }
    }


    /// <summary>
    /// 监视数值变化组件,分发监听
    /// </summary>
    public class NumericWatcherComponent : Entity, IAwakeSystem
    {
        public static NumericWatcherComponent Instance { get; set; }

        public Dictionary<int, List<NumericWatcherInfo>> allWatchers;

        public void Awake(Entity o)
        {
            NumericWatcherComponent.Instance = this;
            Init();
        }


        private void Init()
        {
            allWatchers = new Dictionary<int, List<NumericWatcherInfo>>();

            HashSet<Type> types = EventSystem.Instance.GetTypes(typeof(NumericWatcherAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(NumericWatcherAttribute), false);

                foreach (object attr in attrs)
                {
                    NumericWatcherAttribute numericWatcherAttribute = (NumericWatcherAttribute)attr;
                    INumericWatcher obj = (INumericWatcher)Activator.CreateInstance(type);
                    NumericWatcherInfo numericWatcherInfo =
                        new NumericWatcherInfo(numericWatcherAttribute.SceneType, obj);
                    if (!allWatchers.ContainsKey(numericWatcherAttribute.NumericType))
                    {
                        allWatchers.Add(numericWatcherAttribute.NumericType, new List<NumericWatcherInfo>());
                    }

                    allWatchers[numericWatcherAttribute.NumericType].Add(numericWatcherInfo);
                }
            }
        }

        public void Run(Unit unit, EventType.NumbericChange args)
        {
            List<NumericWatcherInfo> list;
            if (!allWatchers.TryGetValue(args.NumericType, out list))
            {
                return;
            }

            SceneType unitDomainSceneType = unit.DomainScene().SceneType;
            foreach (NumericWatcherInfo numericWatcher in list)
            {
                if (numericWatcher.SceneType != unitDomainSceneType)
                {
                    continue;
                }

                numericWatcher.INumericWatcher.Run(unit, args);
            }
        }
    }
}