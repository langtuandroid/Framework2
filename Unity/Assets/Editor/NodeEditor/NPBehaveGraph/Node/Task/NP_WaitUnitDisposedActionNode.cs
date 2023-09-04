using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/等待Unit销毁", typeof(SkillGraph))]
[NodeMenuItem("NPBehave行为树/Task/等待Unit销毁", typeof(NPBehaveGraph))]
public class NP_WaitUnitDisposedActionNode : NP_TaskNodeBase
{
    public override string name => "等待Unit销毁";

    public NP_ActionNodeData NP_ActionNodeData = new() { NpClassForStoreAction = new NP_WaitUnitDisposedAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override string CreateNodeName => nameof(NP_WaitUnitDisposedAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}