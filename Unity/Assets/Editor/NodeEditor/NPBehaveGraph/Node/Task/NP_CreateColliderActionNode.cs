using System;
using GraphProcessor;
using Action = NPBehave.Action;

[NodeMenuItem("NPBehave行为树/Task/创建碰撞体", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/创建碰撞体", typeof (SkillGraph))]
public class NP_CreateColliderActionNode: NP_TaskNodeBase
{
    public override string name => "创建碰撞体";

    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_CreateColliderAction() };

    // [BsonIgnore]
    // [LabelText("碰撞体附带的子图数据(预览用)")]
    // public List<ChildrenNodeDatas> ChildrenNodeDatas = new List<ChildrenNodeDatas>();

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }

    public override string CreateNodeName => nameof(NP_CreateColliderAction);

    public override void Debug_SetNodeData(object data)
    {
        NP_ActionNodeData = (NP_ActionNodeData)data;
    }
}
