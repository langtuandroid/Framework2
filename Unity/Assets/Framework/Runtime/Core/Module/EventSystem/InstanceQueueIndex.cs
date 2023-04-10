using System;
using System.Collections.Generic;

namespace Framework
{
    public enum InstanceQueueIndex
    {
        None = -1,
        Update,
        LateUpdate,
        RendererUpdate,
        BattleUpdate,
        Max,
    }

    public static class InstanceQueueMap
    {
        public static Dictionary<Type, InstanceQueueIndex> InstanceQueueMapDic =
            new Dictionary<Type, InstanceQueueIndex>()
            {
                { typeof(IUpdateSystem), InstanceQueueIndex.Update },
                { typeof(ILateUpdateSystem), InstanceQueueIndex.LateUpdate },
                { typeof(IRendererUpdateSystem), InstanceQueueIndex.RendererUpdate },
            };
    }
}