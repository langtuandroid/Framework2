using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/运行其他行为树", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/运行其他行为树", typeof(SkillGraph))]
public class NP_RunOtherTreeNode : NP_TaskNodeBase
{
    public override string name => "运行其他行为树";

    public NP_ActionNodeData NP_ActionNodeData = new() { NpClassForStoreAction = new NP_RunOtherTreeAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override string CreateNodeName => nameof(NP_RunOtherTreeAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}