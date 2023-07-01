using Framework;
using Sirenix.OdinInspector;
using Vector3 = UnityEngine.Vector3;
using WaitUntil = NPBehave.WaitUntil;

public class NP_WaitUntilApproachTargetAction : NP_CalssForStoreWaitUntilAction
{
    [HideInEditorMode] private WaitUntil m_WaitNode;

    [LabelText("目标位置")]
    public NP_BlackBoardRelationData<Vector3> TargetPosId = new ();

    [LabelText("移动距目标最短距离")] public BlackboardOrValue_Float EndDis = new BlackboardOrValue_Float(0.1f);
    
    protected override bool UntilFunc()
    {
        if (string.IsNullOrEmpty(TargetPosId.BBKey)) return true;
        return TargetPosId.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard()).SqrDistance(BelongToUnit.Position) <
               EndDis.GetValue(BelongtoRuntimeTree.GetBlackboard());
    }

}