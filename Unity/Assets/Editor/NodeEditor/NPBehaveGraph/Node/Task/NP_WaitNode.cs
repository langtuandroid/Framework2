using System;
using GraphProcessor;
using NPBehave;

[NodeMenuItem("NPBehave行为树/Task/Wait", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/Wait", typeof(SkillGraph))]
public class NP_WaitNode : NP_TaskNodeBase
{
    public override string name => "等待节点";

    public NP_WaitNodeData NP_WaitNodeData = new NP_WaitNodeData { };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_WaitNodeData;
    }

    public override string CreateNodeName => nameof(Wait);

    public override void Debug_SetNodeData(object data)
    {
        NP_WaitNodeData = (NP_WaitNodeData)data;
    }
}