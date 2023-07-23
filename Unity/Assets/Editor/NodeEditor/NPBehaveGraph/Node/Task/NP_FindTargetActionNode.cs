using System;
using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/寻找目标", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/寻找目标", typeof(SkillGraph))]
public class NP_FindTargetActionNode : NP_TaskNodeBase
{
    public override string name => "寻找目标";

    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_FindTargetAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override string CreateNodeName => nameof(NP_FindTargetAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}