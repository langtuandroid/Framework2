using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/等待接近目标点", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/等待接近目标点", typeof(SkillGraph))]
public class NP_WaitUntilApproachTargetNode : NP_TaskNodeBase
{
    public override string name => "等待接近目标点";

    public NP_ActionNodeData NP_WaitNodeData = new()
    {
        NpClassForStoreAction = new NP_WaitUntilApproachTargetAction()
    };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_WaitNodeData;
    }

    public override string CreateNodeName => nameof(NP_WaitUntilApproachTargetAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_WaitNodeData = (NP_ActionNodeData)data;
    }
}