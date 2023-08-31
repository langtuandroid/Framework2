using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_ServiceNodeData : NP_NodeDataBase
{
    [HideInEditorMode] public Service m_Service;

    [LabelText("委托执行时间间隔")] public float interval;

    public NP_ClassForStoreAction NpClassForStoreAction;

    public override NodeType BelongNodeType => NodeType.Decorator;

    public override Node NP_GetNode()
    {
        return this.m_Service;
    }

    public override Decorator CreateDecoratorNode(Unit unit,NP_RuntimeTree runtimeTree, Node node)
    {
        NpClassForStoreAction.BelongToUnit = unit;
        this.NpClassForStoreAction.BelongtoRuntimeTree = runtimeTree;
        this.m_Service = new Service(interval, this.NpClassForStoreAction.GetActionToBeDone(), node);
        return this.m_Service;
    }
}