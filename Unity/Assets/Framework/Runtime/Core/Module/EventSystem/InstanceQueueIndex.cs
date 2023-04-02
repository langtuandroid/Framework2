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
        Max,
    }

    public static class InstanceQueueMap
    {
        public static IReadOnlyDictionary<Type, InstanceQueueIndex> InstanceQueueMapDic =
            new Dictionary<Type, InstanceQueueIndex>()
            {
                { typeof(IUpdateSystem), InstanceQueueIndex.Update },
                { typeof(ILateUpdateSystem), InstanceQueueIndex.LateUpdate },
                { typeof(IRendererUpdateSystem), InstanceQueueIndex.RendererUpdate },
            };
    }
}