using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/等待技能cd", typeof(SkillGraph))]
public class NP_WaitSkillCdActionNode : NP_TaskNodeBase
{
    public override string name => "等待技能cd";

    public NP_ActionNodeData NP_ActionNodeData = new() { NpClassForStoreAction = new NP_WaitSkillCdAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override string CreateNodeName => nameof(NP_WaitSkillCdAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}