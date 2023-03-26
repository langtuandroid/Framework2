using System;

namespace Framework
{
    // 逻辑计算update 会以较低的帧率来跑
    public interface IUpdate
    {
    }

    public interface IUpdateSystem : ISystemType
    {
        void Run(Entity o, float deltaTime);
    }

    [ObjectSystem]
    public abstract class UpdateSystem<T> : IUpdateSystem where T : Entity, IUpdate
    {
        void IUpdateSystem.Run(Entity o, float deltaTime)
        {
            this.Update((T)o, deltaTime);
        }

        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IUpdateSystem);
        }

        InstanceQueueIndex ISystemType.GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.Update;
        }

        protected abstract void Update(T self, float deltaTime);
    }
}