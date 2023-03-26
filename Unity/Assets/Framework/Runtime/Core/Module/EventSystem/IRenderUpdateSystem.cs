using System;

namespace Framework
{
    // 逻辑计算update 会以较低的帧率来跑
    public interface IRendererUpdate
    {
    }

    public interface IRendererUpdateSystem : ISystemType
    {
        void Run(Entity o, float deltaTime);
    }

    [ObjectSystem]
    public abstract class RendererUpdateSystem<T> : IRendererUpdateSystem where T : Entity, IRendererUpdate
    {
        void IRendererUpdateSystem.Run(Entity o, float deltaTime)
        {
            this.Update((T)o, deltaTime);
        }

        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IRendererUpdateSystem);
        }

        InstanceQueueIndex ISystemType.GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.RendererUpdate;
        }

        protected abstract void Update(T self, float deltaTime);
    } 
}