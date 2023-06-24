using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/运行其他行为树", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/运行其他行为树", typeof(SkillGraph))]
public class NP_RunAnotherTreeNode : NP_TaskNodeBase
{
    public override string name => "运行其他行为树";

    public NP_RunAnotherTreeData NP_WaitNodeData = new ();

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_WaitNodeData;
    }
}