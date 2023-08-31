using System;
using GraphProcessor;
using Action = NPBehave.Action;

[NodeMenuItem("NPBehave行为树/Task/添加BuffAction", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/添加BuffAction", typeof (SkillGraph))]
public class NP_AddBuffActionNode: NP_TaskNodeBase
{
    public override string name => "添加BuffAction";

    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_AddBuffToSpecifiedUnitAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override string CreateNodeName => nameof(NP_AddBuffToSpecifiedUnitAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}