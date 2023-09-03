using System;
using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/加载子行为树", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/加载子行为树", typeof(SkillGraph))]
public class NP_LoadSubTreeNode : NP_TaskNodeBase
{
    public override string name => "加载子行为树";

    public NP_LoadSubTreeData NpLoadSubTreeData = new();

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpLoadSubTreeData;
    }

    public override string CreateNodeName { get; }

    public override void Debug_SetNodeData(object data)
    {
        NpLoadSubTreeData = (NP_LoadSubTreeData)data;
    }
}