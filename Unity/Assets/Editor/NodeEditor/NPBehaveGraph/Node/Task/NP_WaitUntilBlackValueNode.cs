    
using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/等待黑板值非默认", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/等待黑板值非默认", typeof(SkillGraph))]
public class NP_WaitUntilBlackValueNode : NP_TaskNodeBase
{
    public override string name => "等待黑板值非默认";

    public NP_WaitUntilNodeData NP_WaitNodeData = new()
    {
        StoreWaitUntilAction = new NP_WaitUntilBlackValueAction()
    };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_WaitNodeData;
    }
}