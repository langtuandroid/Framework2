using Framework;
using NPBehave;
using Sirenix.OdinInspector;
using Vector3 = UnityEngine.Vector3;

public class NP_WaitUntilApproachTargetAction : NP_CalssForStoreWaitUntilAction
{
    [HideInEditorMode] private WaitUntil m_WaitNode;

    [LabelText("目标位置")]
    public NP_BlackBoardRelationData<Vector3> TargetPosId = new ();

    protected override bool UntilFunc()
    {
        if (string.IsNullOrEmpty(TargetPosId.BBKey)) return true;
        return TargetPosId.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard()).SqrDistance(BelongToUnit.Position) < 0.1f;
    }
}