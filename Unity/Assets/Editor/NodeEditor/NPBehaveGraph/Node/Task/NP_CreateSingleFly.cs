using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/发射单一飞行物", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/发射单一飞行物", typeof (SkillGraph))]
public class NP_CreateSingleFly : NP_TaskNodeBase
{
     public override string name => "发射单一飞行物";
 
     public NP_ActionNodeData NP_ActionNodeData =
         new NP_ActionNodeData() { NpClassForStoreAction = new NP_AddBuffAction() };
 
     public override NP_NodeDataBase NP_GetNodeData()
     {
         return NP_ActionNodeData;
     }       
}