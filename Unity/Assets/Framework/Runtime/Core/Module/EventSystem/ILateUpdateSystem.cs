namespace Framework
{
    public interface ILateUpdateSystem : ISystemType
    {
        void LateUpdate(Entity o);
    }
}