using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/技能结束", typeof(SkillGraph))]
public class NP_EndSkillActionNode : NP_TaskNodeBase
{
    public override string name => "技能结束";

    public NP_ActionNodeData NP_ActionNodeData = new() { NpClassForStoreAction = new NP_EndSkillAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override string CreateNodeName => nameof(NP_EndSkillAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}