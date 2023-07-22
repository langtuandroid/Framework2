using GraphProcessor;

[NodeMenuItem("NPBehave行为树/Task/播放动画", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/播放动画", typeof (SkillGraph))]
public class NP_PlayAnimActionNode : NP_TaskNodeBase
{
     public override string name => "播放动画";
 
     public NP_ActionNodeData NP_ActionNodeData =
         new NP_ActionNodeData() { NpClassForStoreAction = new NP_PlayAnimAction() };
      
     public override NP_NodeDataBase NP_GetNodeData()
     {
         return NP_ActionNodeData;
     }

     public override void Debug_SetNodeData(object data)
     {
         NP_ActionNodeData = (NP_ActionNodeData)data;
     }
}