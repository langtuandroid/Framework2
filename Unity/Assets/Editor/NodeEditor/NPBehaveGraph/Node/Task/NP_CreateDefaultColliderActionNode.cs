using Framework;
using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/创建默认碰撞体", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/创建默认碰撞体", typeof (SkillGraph))]
public class NP_CreateDefaultColliderActionNode : NP_TaskNodeBase
{
    public override string name => "创建默认碰撞体";
        
    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_CreateDefaultColliderAction() };
        
    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}