using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/销毁Unit", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/销毁Unit", typeof(SkillGraph))]
public class NP_DestroyUnitActionNode : NP_TaskNodeBase
{
    public override string name => "销毁Unit";

    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_DestroyUnitAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}