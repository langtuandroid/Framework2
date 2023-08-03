using System;
using Sirenix.OdinInspector;

[Title("寻找目标", TitleAlignment = TitleAlignments.Centered)]
public class NP_FindTargetAction : NP_ClassForStoreAction
{
    [LabelText("目标")]
    public NP_BlackBoardRelationData<long> TargetInsId = new ();
    [LabelText("阵营")]
    public RoleCast RoleCast;
    [LabelText("标签")]
    public RoleTag RoleTag;

    [LabelText("多少范围内")] public float Distance;
    
    public override Action GetActionToBeDone()
    {
        return FindTarget;
    }

    private void FindTarget()
    {
        if (BelongToUnit.GetComponent<FindTargetComponent>().FindTarget(RoleCast, RoleTag, Distance, out long id))
        {
            TargetInsId.SetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard(), id);
        }
    }
}