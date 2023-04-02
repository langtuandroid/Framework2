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