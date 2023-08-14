using System;
using Framework;
using Sirenix.OdinInspector;
using Action = NPBehave.Action;

[Title("寻找目标", TitleAlignment = TitleAlignments.Centered)]
public class NP_FindTargetAction : NP_ClassForStoreAction
{
    [LabelText("目标")]
    public NP_BlackBoardRelationData<long> TargetInsId = new();

    [LabelText("阵营")]
    public RoleCast RoleCast;

    [LabelText("标签")]
    public RoleTag RoleTag;

    [LabelText("多少范围内")]
    public BlackboardOrValue_Float Distance;

    public override Func<bool, Action.Result> GetFunc2ToBeDone()
    {
        return FindTarget;
    }

    private Action.Result FindTarget(bool isCancel)
    {
        if (isCancel)
        {
            return Action.Result.SUCCESS;
        }

        if (BelongToUnit.GetComponent<FindTargetComponent>().FindTarget(RoleCast, RoleTag,
                Distance.GetValue(BelongtoRuntimeTree.GetBlackboard()), out long id))
        {
            TargetInsId.SetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard(), id);
            return Action.Result.SUCCESS;
        }

        return Action.Result.PROGRESS;
    }
}