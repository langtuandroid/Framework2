using System;
using GraphProcessor;
using Sirenix.OdinInspector;

[NodeMenuItem("NPBehave行为树/Task/寻路到目标点", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/寻路到目标点", typeof (SkillGraph))]
public class NP_MoveToTargetActionNode : NP_TaskNodeBase
{
    public override string name => "寻路到目标点";

    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_MoveToTargetAction() };
     
    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override string CreateNodeName => nameof(NP_MoveToTargetAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}