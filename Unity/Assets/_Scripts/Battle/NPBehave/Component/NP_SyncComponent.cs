using NPBehave;

namespace Framework
{
    public class NP_SyncComponent : Entity , IAwakeSystem
    {
        public SyncContext SyncContext;
        public void Awake()
        {
            SyncContext = new SyncContext();
        }
    }
}