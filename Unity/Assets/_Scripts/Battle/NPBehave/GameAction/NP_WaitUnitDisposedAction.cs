using System;
using Framework;
using Action = NPBehave.Action;

public class NP_WaitUnitDisposedAction : NP_ClassForStoreAction
{
    public NP_BlackBoardRelationData<long> UnitId = new NP_BlackBoardRelationData<long>();
    private Unit unit;

    public override Func<bool, Action.Result> GetFunc2ToBeDone()
    {
        return UntilFunc;
    }

    private Action.Result UntilFunc(bool isCancel)
    {
        if (isCancel)
        {
            return Action.Result.SUCCESS;
        }

        if (unit == null)
        {
            unit = BelongToUnit.Domain.GetComponent<UnitComponent>()
                .Get(UnitId.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard()));
        }

        if (unit.IsDisposed)
        {
            return Action.Result.SUCCESS;
        }

        return Action.Result.PROGRESS;
    }
}