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
    
    [ShowIf("@!IsFollowTarget || (IsFollowTarget && IsFlyTrigger)")]
    [LabelText("目标")]
    public NP_BlackBoardRelationData<long> TriggeredUnit = new ();

    [LabelText("是否跟随目标")]
    public bool IsFollowTarget;

    [ShowIf(nameof(IsFollowTarget))] [LabelText("目标")]
    public NP_BlackBoardRelationData<long> Target = new();

    [HideIf(nameof(IsFollowTarget))]
    [LabelText("相对于角色的发射方向")]
    public float2 FlyDir;

    [ShowIf(nameof(IsFollowTarget))]
    [LabelText("是否在飞行途中能打中物体")]
    public bool IsFlyTrigger;
    
    public override Action GetActionToBeDone()
    {
        this.Action = this.Create;
        return this.Action;
    }

    private void Create()
    {
    }
}