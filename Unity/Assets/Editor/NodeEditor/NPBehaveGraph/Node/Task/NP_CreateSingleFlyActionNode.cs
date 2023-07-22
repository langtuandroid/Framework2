using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/创建单一飞行物", typeof (SkillGraph))]
public class NP_CreateSingleFlyActionNode : NP_TaskNodeBase
{
    public override string name => "创建单一飞行物";
                
    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_CreateSingleFlyAction() };
                
    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}