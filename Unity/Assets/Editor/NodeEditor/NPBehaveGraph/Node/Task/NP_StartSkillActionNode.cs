using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/技能开始", typeof(SkillGraph))]
public class NP_StartSkillActionNode : NP_TaskNodeBase
{
    public override string name => "技能开始";

    public NP_ActionNodeData NP_ActionNodeData = new() { NpClassForStoreAction = new NP_StartSkillAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override string CreateNodeName => nameof(NP_StartSkillAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}