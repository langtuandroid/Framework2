using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/指定Unit添加Buff", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/指定Unit添加Buff", typeof(SkillGraph))]
public class NP_AddBuffToSpecifiedUnitActionNode : NP_TaskNodeBase
{
    public override string name => "指定Unit添加Buff";

    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_AddBuffToSpecifiedUnitAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }
}