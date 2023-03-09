using System.Collections.Generic;
using Framework;

namespace Framework
{
    /// <summary>
    /// 战斗系统中的事件系统组件，一场战斗挂载一个
    /// </summary>
    public class BattleEventSystemComponent : Entity, IDestroy
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
    }
}