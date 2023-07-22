using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/设置Unit的Transform信息", typeof(SkillGraph))]
[NodeMenuItem("NPBehave行为树/Task/设置Unit的Transform信息", typeof(NPBehaveGraph))]
public class NP_SetUnitTransformActionNode : NP_TaskNodeBase
{
    /// <summary>
    /// 内部ID
    /// </summary>
    public override string name => "设置Unit的Transform信息";
        
    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_SetUnitTransformAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}