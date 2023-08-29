using System;
using Framework;
using Sirenix.OdinInspector;

/// <summary>
/// 这里默认修改自己的黑板值
/// </summary>
[Title("修改黑板值", TitleAlignment = TitleAlignments.Centered)]
public class ChangeBlackValueAction : SkillBaseAction
{
    public override Action GetActionToBeDone()
    {
        return ChangeBlackBoard;
    }

    public void ChangeBlackBoard()
    {
        this.NPBalckBoardRelationData.SetBlackBoardValue(this.BelongtoRuntimeTree.GetBlackboard());
    }
}