    
using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("添加默认碰撞", TitleAlignment = TitleAlignments.Centered)]
public class NP_AddDefaultColliderAction : NP_ClassForStoreAction
{

    [LabelText("碰撞体id")] public int ColliderId;

    [LabelText("持续时间")] public float Duration;

    /// <summary>
    /// 只在跟随Unit时有效，因为不跟随Unit说明是世界空间的碰撞体，
    /// </summary>
    [LabelText("相对于释放者的偏移量")] public Vector3 Offset;

    /// <summary>
    /// 只在不跟随Unit时有效，跟随Unit代表使用BelongToUnit的Transform
    /// </summary>
    [LabelText("相对于释放者的旋转角度")] public float Angle;

    public override Action GetActionToBeDone()
    {
        this.Action = this.CreateColliderData;
        return this.Action;
    }

    private void CreateColliderData()
    {

    }
}