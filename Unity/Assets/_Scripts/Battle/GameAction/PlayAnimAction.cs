using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using Action = NPBehave.Action;

[Title("播放动画", TitleAlignment = TitleAlignments.Centered)]
public class PlayAnimAction : SkillBaseAction
{
    public BlackboardOrValue_String AnimName = new("播放动画的名称");

    [LabelText("完成的帧率")]
    [Tooltip("-1为直接完成")]
    public int FinishFrame = -1;

    private bool isPlayAnim = false;

    public override Func<bool, Action.Result> GetFunc2ToBeDone()
    {
        return IsAnimUntilFrame;
    }

    private Action.Result IsAnimUntilFrame(bool isCancel)
    {
        if (isCancel)
        {
            return Action.Result.SUCCESS;
        }

        if (!isPlayAnim)
        {
            // play
        }

        // 判断是否到跳过的帧率
        if (FinishFrame == -1)
        {
            return Action.Result.SUCCESS;
        }

        return Action.Result.SUCCESS;
    }

    private void PlayAnim()
    {
//        BelongToUnit.GetComponent<PlayAnimComponent>().PlayAnim(AnimName.GetValue(BelongtoRuntimeTree.GetBlackboard()));
    }
}