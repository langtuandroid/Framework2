﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class EventSystem : Singleton<EventSystem>, ISingletonUpdate, ISingletonLateUpdate, ISingletonRendererUpdate
    {
        private class OneTypeSystems
        {
            public readonly UnOrderMultiMap<Type, object> Map = new();

            // 这里不用hash，数量比较少，直接for循环速度更快
            public readonly bool[] QueueFlag = new bool[(int)InstanceQueueIndex.Max];
        }

        private class TypeSystems
        {
            private readonly Dictionary<Type, OneTypeSystems> typeSystemsMap = new();

            public OneTypeSystems GetOrCreateOneTypeSystems(Type type)
            {
                OneTypeSystems systems = null;
                this.typeSystemsMap.TryGetValue(type, out systems);
                if (systems != null)
                {
                    return systems;
                }

                systems = new OneTypeSystems();
                this.typeSystemsMap.Add(type, systems);
                return systems;
            }

            public OneTypeSystems GetOneTypeSystems(Type type)
            {
                OneTypeSystems systems = null;
                this.typeSystemsMap.TryGetValue(type, out systems);
                return systems;
            }

            public List<object> GetSystems(Type type, Type systemType)
            {
                OneTypeSystems oneTypeSystems = null;
                if (!this.typeSystemsMap.TryGetValue(type, out oneTypeSystems))
                {
                    return null;
                }

                if (!oneTypeSystems.Map.TryGetValue(systemType, out List<object> systems))
                {
                    return null;
                }

                return systems;
            }
        }

        private class EventInfo
        {
            public IEvent IEvent { get; }

            public EventInfo(IEvent iEvent, SceneType sceneType)
            {
                this.IEvent = iEvent;
            }
        }

        private readonly Dictionary<string, Type> allTypes = new();

        private readonly UnOrderMultiMapSet<Type, Type> attribute2Types = new();
        private readonly UnOrderMultiMapSet<Type, (BaseAttribute attribute, Type type)> attribute2TypesAndAttribute = new();

        private readonly Dictionary<Type, List<EventInfo>> allEvents = new();

        private Dictionary<Type, Dictionary<int, object>> allInvokes = new();

        private TypeSystems typeSystems = new();

        private readonly Queue<long>[] queues = new Queue<long>[(int)InstanceQueueIndex.Max];

        public EventSystem()
        {
            for (int i = 0; i < this.queues.Length; i++)
            {
                this.queues[i] = new Queue<long>();
            }
        }

        public void Add(Dictionary<string, Type> addTypes)
        {
            foreach ((string fullName, Type type) in addTypes)
            {
                this.allTypes[fullName] = type;

                if (type.IsAbstract)
                {
                    continue;
                }

                // 记录所有的有BaseAttribute标记的的类型
                object[] objects = type.GetCustomAttributes(typeof(BaseAttribute), true);

                foreach (object o in objects)
                {
                    this.attribute2Types.Add(o.GetType(), type);
                    this.attribute2TypesAndAttribute.Add(o.GetType(), (o as BaseAttribute, type));
                }
            }

        }

        /// <summary>
        /// 需要在所有程序集的类都加入完后才能初始化
        /// </summary>
        public void InitType()
        {
            this.typeSystems = new TypeSystems();

            foreach (Type type in this.GetTypes(typeof(ObjectSystemAttribute)))
            {
                object obj = Activator.CreateInstance(type);

                if (obj is ISystemType iSystemType)
                {
                    OneTypeSystems oneTypeSystems = this.typeSystems.GetOrCreateOneTypeSystems(iSystemType.Type());
                    oneTypeSystems.Map.Add(iSystemType.SystemType(), obj);
                    InstanceQueueIndex index = iSystemType.GetInstanceQueueIndex();
                    if (index > InstanceQueueIndex.None && index < InstanceQueueIndex.Max)
                    {
                        oneTypeSystems.QueueFlag[(int)index] = true;
                    }
                }
            }
            
            this.allEvents.Clear();
            foreach (Type type in attribute2Types[typeof(EventAttribute)])
            {
                IEvent obj = Activator.CreateInstance(type) as IEvent;
                if (obj == null)
                {
                    throw new Exception($"type not is AEvent: {type.Name}");
                }

                object[] attrs = type.GetCustomAttributes(typeof(EventAttribute), false);
                foreach (object attr in attrs)
                {
                    EventAttribute eventAttribute = attr as EventAttribute;

                    Type eventType = obj.Type;

                    EventInfo eventInfo = new(obj, eventAttribute.SceneType);

                    if (!this.allEvents.ContainsKey(eventType))
                    {
                        this.allEvents.Add(eventType, new List<EventInfo>());
                    }

                    this.allEvents[eventType].Add(eventInfo);
                }
            }

            foreach (Type type in attribute2Types[typeof(InvokeAttribute)])
            {
                object obj = Activator.CreateInstance(type);
                IInvoke iInvoke = obj as IInvoke;
                if (iInvoke == null)
                {
                    throw new Exception($"type not is callback: {type.Name}");
                }

                object[] attrs = type.GetCustomAttributes(typeof(InvokeAttribute), false);
                foreach (object attr in attrs)
                {
                    if (!this.allInvokes.TryGetValue(iInvoke.Type, out var dict))
                    {
                        dict = new Dictionary<int, object>();
                        this.allInvokes.Add(iInvoke.Type, dict);
                    }

                    InvokeAttribute invokeAttribute = attr as InvokeAttribute;

                    try
                    {
                        dict.Add(invokeAttribute.Type, obj);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"action type duplicate: {iInvoke.Type.Name} {invokeAttribute.Type}", e);
                    }
                }
            }
        }

        public HashSet<Type> GetTypes(Type systemAttributeType)
        {
            if (!this.attribute2Types.TryGetValue(systemAttributeType, out var result))
            {
                result = new HashSet<Type>();
            }

            return result;
        }
        
        public HashSet<(BaseAttribute attribute, Type type)> GetTypesAndAttribute(Type systemAttributeType)
        {
            if (!this.attribute2TypesAndAttribute.TryGetValue(systemAttributeType, out var result))
            {
                result = new HashSet<(BaseAttribute attribute, Type type)>();
            }

            return result;
        }

        public Dictionary<string, Type> GetTypes()
        {
            return allTypes;
        }

        public Type GetType(string typeName)
        {
            return this.allTypes[typeName];
        }

        public void RegisterSystem(Entity component)
        {
            Type type = component.GetType();

            OneTypeSystems oneTypeSystems = this.typeSystems.GetOneTypeSystems(type);
            if (oneTypeSystems == null)
            {
                return;
            }

            for (int i = 0; i < oneTypeSystems.QueueFlag.Length; ++i)
            {
                if (!oneTypeSystems.QueueFlag[i])
                {
                    continue;
                }

                this.queues[i].Enqueue(component.InstanceId);
            }
        }

        public void Deserialize(Entity component)
        {
            List<object> iDeserializeSystems =
                this.typeSystems.GetSystems(component.GetType(), typeof(IDeserializeSystem));
            if (iDeserializeSystems == null)
            {
                return;
            }

            foreach (IDeserializeSystem deserializeSystem in iDeserializeSystems)
            {
                if (deserializeSystem == null)
                {
                    continue;
                }

                try
                {
                    deserializeSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        // GetComponentSystem
        public void GetComponent(Entity entity, Entity component)
        {
            List<object> iGetSystem = this.typeSystems.GetSystems(entity.GetType(), typeof(IGetComponentSystem));
            if (iGetSystem == null)
            {
                return;
            }

            foreach (IGetComponentSystem getSystem in iGetSystem)
            {
                if (getSystem == null)
                {
                    continue;
                }

                try
                {
                    getSystem.Run(entity, component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        // AddComponentSystem
        public void AddComponent(Entity entity, Entity component)
        {
            List<object> iAddSystem = this.typeSystems.GetSystems(entity.GetType(), typeof(IAddComponentSystem));
            if (iAddSystem == null)
            {
                return;
            }

            foreach (IAddComponentSystem addComponentSystem in iAddSystem)
            {
                if (addComponentSystem == null)
                {
                    continue;
                }

                try
                {
                    addComponentSystem.Run(entity, component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake(Entity component)
        {
            List<object> iAwakeSystems = this.typeSystems.GetSystems(component.GetType(), typeof(IAwakeSystem));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeSystem aAwakeSystem in iAwakeSystems)
            {
                if (aAwakeSystem == null)
                {
                    continue;
                }

                try
                {
                    aAwakeSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake<P1>(Entity component, P1 p1)
        {
            List<object> iAwakeSystems = this.typeSystems.GetSystems(component.GetType(), typeof(IAwakeSystem<P1>));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeSystem<P1> aAwakeSystem in iAwakeSystems)
            {
                if (aAwakeSystem == null)
                {
                    continue;
                }

                try
                {
                    aAwakeSystem.Run(component, p1);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake<P1, P2>(Entity component, P1 p1, P2 p2)
        {
            List<object> iAwakeSystems = this.typeSystems.GetSystems(component.GetType(), typeof(IAwakeSystem<P1, P2>));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeSystem<P1, P2> aAwakeSystem in iAwakeSystems)
            {
                if (aAwakeSystem == null)
                {
                    continue;
                }

                try
                {
                    aAwakeSystem.Run(component, p1, p2);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake<P1, P2, P3>(Entity component, P1 p1, P2 p2, P3 p3)
        {
            List<object> iAwakeSystems =
                this.typeSystems.GetSystems(component.GetType(), typeof(IAwakeSystem<P1, P2, P3>));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeSystem<P1, P2, P3> aAwakeSystem in iAwakeSystems)
            {
                if (aAwakeSystem == null)
                {
                    continue;
                }

                try
                {
                    aAwakeSystem.Run(component, p1, p2, p3);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake<P1, P2, P3, P4>(Entity component, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            List<object> iAwakeSystems =
                this.typeSystems.GetSystems(component.GetType(), typeof(IAwakeSystem<P1, P2, P3, P4>));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeSystem<P1, P2, P3, P4> aAwakeSystem in iAwakeSystems)
            {
                if (aAwakeSystem == null)
                {
                    continue;
                }

                try
                {
                    aAwakeSystem.Run(component, p1, p2, p3, p4);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Load()
        {
            Queue<long> queue = this.queues[(int)InstanceQueueIndex.Load];
            int count = queue.Count;
            while (count-- > 0)
            {
                long instanceId = queue.Dequeue();
                Entity component = Root.Instance.Get(instanceId);
                if (component == null)
                {
                    continue;
                }

                if (component.IsDisposed)
                {
                    continue;
                }

                List<object> iLoadSystems = this.typeSystems.GetSystems(component.GetType(), typeof(ILoadSystem));
                if (iLoadSystems == null)
                {
                    continue;
                }

                queue.Enqueue(instanceId);

                foreach (ILoadSystem iLoadSystem in iLoadSystems)
                {
                    try
                    {
                        iLoadSystem.Run(component);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
        }

        public void Destroy(Entity component)
        {
            List<object> iDestroySystems = this.typeSystems.GetSystems(component.GetType(), typeof(IDestroySystem));
            if (iDestroySystems == null)
            {
                return;
            }

            foreach (IDestroySystem iDestroySystem in iDestroySystems)
            {
                if (iDestroySystem == null)
                {
                    continue;
                }

                try
                {
                    iDestroySystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Update(float deltaTime)
        {
            Queue<long> queue = this.queues[(int)InstanceQueueIndex.Update];
            int count = queue.Count;
            while (count-- > 0)
            {
                long instanceId = queue.Dequeue();
                Entity component = Root.Instance.Get(instanceId);
                if (component == null)
                {
                    continue;
                }

                if (component.IsDisposed)
                {
                    continue;
                }

                List<object> iUpdateSystems = this.typeSystems.GetSystems(component.GetType(), typeof(IUpdateSystem));
                if (iUpdateSystems == null)
                {
                    continue;
                }

                queue.Enqueue(instanceId);

                foreach (IUpdateSystem iUpdateSystem in iUpdateSystems)
                {
                    try
                    {
                        iUpdateSystem.Run(component, deltaTime);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }

        }
        
        public void RendererUpdate(float deltaTime)
        {
            long currentTime = TimeInfo.Instance.ClientNow();
            Queue<long> queue = this.queues[(int)InstanceQueueIndex.RendererUpdate];
            int count = queue.Count;
            while (count-- > 0)
            {
                long instanceId = queue.Dequeue();
                Entity component = Root.Instance.Get(instanceId);
                if (component == null)
                {
                    continue;
                }

                if (component.IsDisposed)
                {
                    continue;
                }

                List<object> iUpdateSystems = this.typeSystems.GetSystems(component.GetType(), typeof(IRendererUpdateSystem));
                if (iUpdateSystems == null)
                {
                    continue;
                }

                queue.Enqueue(instanceId);

                foreach (IRendererUpdateSystem iUpdateSystem in iUpdateSystems)
                {
                    try
                    {
                        iUpdateSystem.Run(component, deltaTime);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }

        }

        public void LateUpdate(float deltaTime)
        {
            Queue<long> queue = this.queues[(int)InstanceQueueIndex.LateUpdate];
            int count = queue.Count;
            while (count-- > 0)
            {
                long instanceId = queue.Dequeue();
                Entity component = Root.Instance.Get(instanceId);
                if (component == null)
                {
                    continue;
                }

                if (component.IsDisposed)
                {
                    continue;
                }

                List<object> iLateUpdateSystems =
                    this.typeSystems.GetSystems(component.GetType(), typeof(ILateUpdateSystem));
                if (iLateUpdateSystems == null)
                {
                    continue;
                }

                queue.Enqueue(instanceId);

                foreach (ILateUpdateSystem iLateUpdateSystem in iLateUpdateSystems)
                {
                    try
                    {
                        iLateUpdateSystem.Run(component);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
        }
        

        public async ETTask PublishAsync<T>(Scene scene, T a) where T : struct
        {
            List<EventInfo> iEvents;
            if (!this.allEvents.TryGetValue(typeof(T), out iEvents))
            {
                return;
            }

            using RecyclableList<ETTask> recyclableList = RecyclableList<ETTask>.Create();

            foreach (EventInfo eventInfo in iEvents)
            {
                if (!(eventInfo.IEvent is AEvent<T> aEvent))
                {
                    Log.Error($"event error: {eventInfo.IEvent.GetType().Name}");
                    continue;
                }

                recyclableList.Add(aEvent.Handle(scene, a));
            }

            try
            {
                await ETTaskHelper.WaitAll(recyclableList);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void Publish<T>(Scene scene, T a) where T : struct
        {
            List<EventInfo> iEvents;
            if (!this.allEvents.TryGetValue(typeof(T), out iEvents))
            {
                return;
            }

            SceneType sceneType = scene.SceneType;
            foreach (EventInfo eventInfo in iEvents)
            {
                if (!(eventInfo.IEvent is AEvent<T> aEvent))
                {
                    Log.Error($"event error: {eventInfo.IEvent.GetType().Name}");
                    continue;
                }

                aEvent.Handle(scene, a).Coroutine();
            }
        }

        // Invoke跟Publish的区别(特别注意)
        // Invoke类似函数，必须有被调用方，否则异常，调用者跟被调用者属于同一模块，比如MoveComponent中的Timer计时器，调用跟被调用的代码均属于移动模块
        // 既然Invoke跟函数一样，那么为什么不使用函数呢? 因为有时候不方便直接调用，比如Config加载，在客户端跟服务端加载方式不一样。比如TimerComponent需要根据Id分发
        // Invoke用在定时器到时间后调用某个函数，且必须有接收方
        // 注意，不要把Invoke当函数使用，这样会造成代码可读性降低，能用函数不要用Invoke
        // publish是事件，抛出去可以没人订阅，调用者跟被调用者属于两个模块，比如任务系统需要知道道具使用的信息，则订阅道具使用事件
        public void Invoke<A>(int type, A args) where A : struct
        {
            if (!this.allInvokes.TryGetValue(typeof(A), out var invokeHandlers))
            {
                throw new Exception($"Invoke error: {typeof(A).Name}");
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                throw new Exception($"Invoke error: {typeof(A).Name} {type}");
            }

            var aInvokeHandler = invokeHandler as AInvokeHandler<A>;
            if (aInvokeHandler == null)
            {
                throw new Exception($"Invoke error, not AInvokeHandler: {typeof(A).Name} {type}");
            }

            aInvokeHandler.Handle(args);
        }

        public T Invoke<A, T>(int type, A args) where A : struct
        {
            if (!this.allInvokes.TryGetValue(typeof(A), out var invokeHandlers))
            {
                throw new Exception($"Invoke error: {typeof(A).Name}");
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                throw new Exception($"Invoke error: {typeof(A).Name} {type}");
            }

            var aInvokeHandler = invokeHandler as AInvokeHandler<A, T>;
            if (aInvokeHandler == null)
            {
                throw new Exception($"Invoke error, not AInvokeHandler: {typeof(T).Name} {type}");
            }

            return aInvokeHandler.Handle(args);
        }

        public void Invoke<A>(A args) where A : struct
        {
            Invoke(0, args);
        }

        public T Invoke<A, T>(A args) where A : struct
        {
            return Invoke<A, T>(0, args);
        }
    }
}