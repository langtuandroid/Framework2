using Framework;
using NPBehave;
using Sirenix.OdinInspector;
using WaitUntil = NPBehave.WaitUntil;

public class NP_WaitUntilNodeData : NP_NodeDataBase
{
    public NP_CalssForStoreWaitUntilAction StoreWaitUntilAction;
    [HideInEditorMode] private WaitUntil waitUntilNode;

    public override NodeType BelongNodeType => NodeType.Task;

    public override Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
    {
        StoreWaitUntilAction.BelongToUnit = unit;
        this.StoreWaitUntilAction.BelongtoRuntimeTree = runtimeTree;
        waitUntilNode = StoreWaitUntilAction._CreateNPBehaveAction();
        return this.waitUntilNode;
    }

    public override Node NP_GetNode()
    {
        return this.waitUntilNode;
    }
}