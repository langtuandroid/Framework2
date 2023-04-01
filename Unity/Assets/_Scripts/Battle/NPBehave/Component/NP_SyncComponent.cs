using NPBehave;

namespace Framework
{
    public class NP_SyncComponent : Entity , IAwake
    {
        public SyncContext SyncContext;
    }
    
    public class NP_SyncComponentAwakeSystem : AwakeSystem<NP_SyncComponent>
    {
        protected override void Awake(NP_SyncComponent self)
        {
            self.SyncContext = new SyncContext();
        }
    }
}