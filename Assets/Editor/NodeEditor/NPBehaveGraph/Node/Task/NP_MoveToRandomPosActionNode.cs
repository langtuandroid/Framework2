using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/寻路到随机位置", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/寻路到随机位置", typeof (SkillGraph))]
public class NP_MoveToRandomPosActionNode: NP_TaskNodeBase
{
    public override string name => "寻路到随机位置";

    public NP_ActionNodeData NP_ActionNodeData =
        new NP_ActionNodeData() { NpClassForStoreAction = new NP_MoveToRandomPosAction() };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_ActionNodeData;
    }
}