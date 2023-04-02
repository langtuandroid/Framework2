using System.Collections.Generic;
using Framework;
using NPBehave;

namespace Framework
{
    /// <summary>
    /// 战斗系统中的事件系统组件，一场战斗挂载一个
    /// </summary>
    public class BattleEventSystemComponent : Entity, IDestroySystem
    {
        public readonly Dictionary<string, LinkedList<ISkillSystemEvent>> AllEvents =
            new Dictionary<string, LinkedList<ISkillSystemEvent>>();

        /// <summary>
        /// 缓存的结点字典
        /// </summary>
        public readonly Dictionary<string, LinkedListNode<ISkillSystemEvent>> CachedNodes =
            new Dictionary<string, LinkedListNode<ISkillSystemEvent>>();

        /// <summary>
        /// 临时结点字典
        /// </summary>
        public readonly Dictionary<string, LinkedListNode<ISkillSystemEvent>> TempNodes =
            new Dictionary<string, LinkedListNode<ISkillSystemEvent>>();


        public void RegisterEvent(string eventId, ISkillSystemEvent e)
        {
            if (!AllEvents.ContainsKey(eventId))
            {
                AllEvents.Add(eventId, new LinkedList<ISkillSystemEvent>());
            }

            AllEvents[eventId].AddLast(e);
        }

        public void UnRegisterEvent(string eventId, ISkillSystemEvent e)
        {
            if (CachedNodes.Count > 0)
            {
                foreach (KeyValuePair<string, LinkedListNode<ISkillSystemEvent>> cachedNode in CachedNodes)
                {
                    //预防极端情况，比如两个不同的事件id订阅了同一个事件处理者
                    if (cachedNode.Value != null && cachedNode.Key == eventId && cachedNode.Value.Value == e)
                    {
                        //注意这里添加的Handler是下一个
                        TempNodes.Add(cachedNode.Key, cachedNode.Value.Next);
                    }
                }

                //把临时结点字典中的目标元素值更新到缓存结点字典
                if (TempNodes.Count > 0)
                {
                    foreach (KeyValuePair<string, LinkedListNode<ISkillSystemEvent>> cachedNode in TempNodes)
                    {
                        CachedNodes[cachedNode.Key] = cachedNode.Value;
                    }

                    //清除临时结点
                    TempNodes.Clear();
                }
            }

            if (AllEvents.ContainsKey(eventId))
            {
                AllEvents[eventId].Remove(e);
                ReferencePool.Free(e);
            }
        }

        public void Run(string type)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    CachedNodes[type] = temp.Next;
                    temp.Value?.Handle();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = CachedNodes[type];
            }

            CachedNodes.Remove(type);
        }

        public void Run<A>(string type, A a)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    CachedNodes[type] = temp.Next;
                    temp.Value?.Handle(a);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = CachedNodes[type];
            }

            CachedNodes.Remove(type);
        }

        public void Run<A, B>(string type, A a, B b)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    CachedNodes[type] = temp.Next;
                    temp.Value?.Handle(a, b);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = CachedNodes[type];
            }

            CachedNodes.Remove(type);
        }

        public void Run<A, B, C>(string type, A a, B b, C c)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    CachedNodes[type] = temp.Next;
                    temp.Value?.Handle(a, b, c);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = CachedNodes[type];
            }

            CachedNodes.Remove(type);
        }

        public void OnDestroy(Entity o)
        {

            AllEvents.Clear();
            CachedNodes.Clear();
            TempNodes.Clear();
        }
    }
}