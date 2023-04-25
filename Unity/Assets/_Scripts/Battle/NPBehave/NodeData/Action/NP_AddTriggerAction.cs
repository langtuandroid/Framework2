using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class NP_AddTriggerAction : NP_ClassForStoreAction
{
    [LabelText("碰撞体的路径")][Required] public string ColliderPath;

    [LabelText("是否位置跟随")] public bool IsFollowPos;

    [LabelText("是否旋转跟随")] public bool IsFollowRot;

    [LabelText("生成点")] [Required] public string HangPoint;

    [LabelText("生成点位置偏移")] public Vector3 HangPointPosOffset;

    [LabelText("生成点旋转偏移")] public Vector3 HangPointRotOffset;

    public override Action GetActionToBeDone()
    {
        this.Action = this.AddTrigger;
        return this.Action;
    }

    private void AddTrigger()
    {
    }
}