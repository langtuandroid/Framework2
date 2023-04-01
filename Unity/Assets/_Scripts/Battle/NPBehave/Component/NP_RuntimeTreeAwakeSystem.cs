using Framework;

public class NP_RuntimeTreeAwakeSystem : AwakeSystem<NP_RuntimeTree, NP_DataSupportor, NP_SyncComponent, Unit>
{
    protected override void Awake(NP_RuntimeTree self, NP_DataSupportor belongNP_DataSupportor,
        NP_SyncComponent npSyncComponent, Unit belongToUnit)
    {
        self.BelongToUnit = belongToUnit;
        self.BelongNP_DataSupportor = belongNP_DataSupportor;
        self.NpSyncComponent = npSyncComponent;
    }
}

public class NP_RuntimeTreeUpdateSysmte : UpdateSystem<NP_RuntimeTree>
{
    protected override void Update(NP_RuntimeTree self, float deltaTime)
    {
        self.GetClock().Update(deltaTime);
    }
}