using System;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;
using Action = NPBehave.Action;
using Exception = System.Exception;

[BoxGroup("行为结点数据")]
[HideLabel]
public class NP_ActionNodeData : NP_NodeDataBase
{
    [HideInEditorMode]
    private Action m_ActionNode;

    public NP_ClassForStoreAction NpClassForStoreAction;

    public override Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
    {
        NpClassForStoreAction.BelongToUnit = unit;
        NpClassForStoreAction.BelongtoRuntimeTree = runtimeTree;
        m_ActionNode = NpClassForStoreAction._CreateNPBehaveAction();
        return m_ActionNode;
    }

    public override NodeType BelongNodeType => NodeType.Task;

    public override Node NP_GetNode()
    {
        return m_ActionNode;
    }
}