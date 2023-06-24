using System;
using Framework;
using Sirenix.OdinInspector;

/// <summary>
/// 这里默认修改自己的黑板值
/// </summary>
[Title("修改黑板值", TitleAlignment = TitleAlignments.Centered)]
public class NP_ChangeBlackValueAction: NP_ClassForStoreAction
{
    public NP_BlackBoardRelationData<object> NPBalckBoardRelationData = new () { WriteOrCompareToBB = true };

    public override Action GetActionToBeDone()
    {
        this.Action = this.ChangeBlackBoard;
        return this.Action;
    }

    public void ChangeBlackBoard()
    {
        this.NPBalckBoardRelationData.SetBlackBoardValue(this.BelongtoRuntimeTree.GetBlackboard());
    }
}