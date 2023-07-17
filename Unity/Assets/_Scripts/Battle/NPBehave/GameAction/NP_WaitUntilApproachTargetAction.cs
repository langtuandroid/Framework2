using Framework;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Vector3 = UnityEngine.Vector3;
using WaitUntil = NPBehave.WaitUntil;

public class NP_WaitUntilApproachTargetAction : NP_CalssForStoreWaitUntilAction
{
    [HideInEditorMode] private WaitUntil m_WaitNode;

    [BoxGroup("/0", false)]
    [LabelText("目标1是自己吗")]
    public bool Target1IsSelf = true;

    [BoxGroup("/0", false)]
    [LabelText("目标1")]
    [HideIf(nameof(Target1IsSelf))]
    public NP_BlackBoardRelationData<long> Target1Id = new ();

    [BoxGroup("/1", false)]
    [LabelText("目标2是固定位置吗")]
    public bool Target2IsPos = false;

    [BoxGroup("/1", false)]
    [LabelText("目标2")]
    [HideIf(nameof(Target2IsPos))]
    public NP_BlackBoardRelationData<long> Target2Id = new ();

    [BoxGroup("/1", false)]
    [LabelText("目标位置")]
    [ShowIf(nameof(Target2IsPos))]
    public NP_BlackBoardRelationData<Vector3> Target2Pos= new ();

    [BoxGroup("/2", false)]
    [LabelText("移动距目标最短距离")] public BlackboardOrValue_Float EndDis = new BlackboardOrValue_Float(0.1f);

    protected override bool UntilFunc()
    {
        float3 target1Pos = Target1IsSelf
            ? BelongToUnit.Position
            : BelongToUnit.Domain.GetComponent<UnitComponent>()
                .Get(Target1Id.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard())).Position;
        float3 target2Pos = Target2IsPos
            ? Target2Pos.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard())
            : BelongToUnit.Domain.GetComponent<UnitComponent>()
                .Get(Target2Id.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard())).Position;
        var result = math.distance(target1Pos, target2Pos) <= EndDis.GetValue(BelongtoRuntimeTree.GetBlackboard());
        return result;
    }
}