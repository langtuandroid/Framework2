using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/等待接近目标点", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/等待接近目标点", typeof(SkillGraph))]
public class NP_WaitUntilApproachTargetNode : NP_TaskNodeBase
{
     public override string name => "等待接近目标点";

     public NP_WaitUntilApproachTargetData NP_WaitNodeData = new NP_WaitUntilApproachTargetData();
 
     public override NP_NodeDataBase NP_GetNodeData()
     {
         return NP_WaitNodeData;
     }       
}