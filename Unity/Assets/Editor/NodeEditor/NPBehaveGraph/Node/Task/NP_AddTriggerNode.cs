using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/添加触发器", typeof(SkillGraph))]
public class NP_AddTriggerNode : NP_TaskNodeBase
{
    public override string name => "添加触发器";

    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_AddTriggerAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }
}