using Framework;
using NPBehave;
using Sirenix.OdinInspector;
using Vector3 = UnityEngine.Vector3;

public class NP_WaitUntilApproachTargetData : NP_NodeDataBase
{

    [HideInEditorMode] private WaitUntil m_WaitNode;

    [LabelText("目标位置")]
    public NP_BlackBoardRelationData<Vector3> TargetPosId = new ();
    
    public override NodeType BelongNodeType => NodeType.Task;

    public override Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
    {
        m_WaitNode = new WaitUntil(null,() =>
        {
            if (string.IsNullOrEmpty(TargetPosId.BBKey)) return true;
            return TargetPosId.GetBlackBoardValue(runtimeTree.GetBlackboard()).SqrDistance(unit.Position) < 0.1f;
        });
        return this.m_WaitNode;
    }

    public override Node NP_GetNode()
    {
        return this.m_WaitNode;
    }
}