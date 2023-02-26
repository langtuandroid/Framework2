using System;

namespace Framework
{
    public interface ISystemType
    {
        Type Type();
        Type SystemType();
        InstanceQueueIndex GetInstanceQueueIndex();
    }
}