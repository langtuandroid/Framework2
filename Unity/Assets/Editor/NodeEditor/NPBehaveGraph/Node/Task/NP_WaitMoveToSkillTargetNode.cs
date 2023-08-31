using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/等待接近技能的目标点", typeof(NPBehaveGraph))]
public class NP_WaitMoveToSkillTargetNode : NP_TaskNodeBase
{
    public override string name => "等待接近技能的目标点";

    public NP_ActionNodeData NP_WaitNodeData = new()
    {
        NpClassForStoreAction = new NP_WaitMoveToSkillTarget()
    };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_WaitNodeData;
    }

    public override string CreateNodeName => nameof(NP_WaitMoveToSkillTarget);

    public override void Debug_SetNodeData(object data)
    {
        NP_WaitNodeData = (NP_ActionNodeData)data;
    }
}