using System;

namespace Framework
{
    public interface IRendererUpdateSystem : ISystemType
    {
        void RenderUpdate(Entity o, float deltaTime);
    }
}