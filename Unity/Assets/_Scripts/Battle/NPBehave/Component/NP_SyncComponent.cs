using NPBehave;

namespace Framework
{
    public class NP_SyncComponent : Entity , IAwakeSystem
    {
        public SyncContext SyncContext;
        public void Awake(Entity o)
        {
            SyncContext = new SyncContext();
        }
    }
}