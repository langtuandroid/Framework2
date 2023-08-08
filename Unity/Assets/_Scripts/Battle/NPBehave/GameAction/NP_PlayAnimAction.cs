using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("播放动画", TitleAlignment = TitleAlignments.Centered)]
public class NP_PlayAnimAction : NP_ClassForStoreAction
{
    public BlackboardOrValue_String AnimName = new("播放动画的名称");

    [LabelText("完成的帧率")]
    [Tooltip("-1为直接完成")]
    public int FinishFrame = -1;

    private bool isPlayAnim = false;

    public override Func<bool, NPBehave.Action.Result> GetFunc2ToBeDone()
    {
        return IsAnimUntilFrame;
    }

    private NPBehave.Action.Result IsAnimUntilFrame(bool isCancel)
    {
        if (!isPlayAnim)
        {
            // play
        }

        // 判断是否到跳过的帧率
        if (FinishFrame == -1)
        {
            return NPBehave.Action.Result.SUCCESS;
        }

        return NPBehave.Action.Result.SUCCESS;
    }

    private void PlayAnim()
    {
//        BelongToUnit.GetComponent<PlayAnimComponent>().PlayAnim(AnimName.GetValue(BelongtoRuntimeTree.GetBlackboard()));
    }
}