using System;
using Framework;
using Sirenix.OdinInspector;
using Unity.Mathematics;

[Title("创建单一飞行物", TitleAlignment = TitleAlignments.Centered)]
public class NP_CreateSingleFlyAction : NP_ClassForStoreAction
{

    [LabelText("飞行物prefab")]
    public string FlyPath;

    [LabelText("飞行物出生点")]
    public string HangPoint;

    [LabelText("飞行速度")]
    public float Speed;

    [LabelText("碰撞体id")]
    public VTD_ColliderId ColliderId;
    
    [LabelText("飞行物的id")]
    public NP_BlackBoardRelationData<long> FlyObjUnit = new ();
    
    [ShowIf("@!IsFollowTarget || (IsFollowTarget && IsFlyingTrigger)")]
    [LabelText("射中的目标")]
    public NP_BlackBoardRelationData<long> TriggeredUnit = new ();

    [LabelText("是否跟随目标")]
    public bool IsFollowTarget;

    [ShowIf(nameof(IsFollowTarget))] [LabelText("跟随的目标")]
    public NP_BlackBoardRelationData<long> FollowTarget = new();

    [HideIf(nameof(IsFollowTarget))]
    [LabelText("相对于角色的发射方向")]
    public float2 FlyDir;

    [ShowIf(nameof(IsFollowTarget))]
    [LabelText("是否在飞行途中能打中物体")]
    public bool IsFlyingTrigger;
    
    public override Action GetActionToBeDone()
    {
        this.Action = this.Create;
        return this.Action;
    }

    private void Create()
    {
        FlyObjHelper.CreateSingleFlyObj(this);
    }
}