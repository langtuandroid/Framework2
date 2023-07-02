using System;
using Framework;
using Sirenix.OdinInspector;
using Unity.Mathematics;

[Title("创建单一飞行物", TitleAlignment = TitleAlignments.Centered)]
public class NP_CreateSingleFlyAction : NP_ClassForStoreAction
{
    [LabelText("飞行物prefab")]
    public BlackboardOrValue_String FlyPath = new BlackboardOrValue_String();

    [LabelText("飞行物出生点")]
    public BlackboardOrValue_String HangPoint = new BlackboardOrValue_String();

    [LabelText("飞行速度")]
    public BlackboardOrValue_Float Speed = new BlackboardOrValue_Float(1);

    [LabelText("碰撞体id")]
    public VTD_ColliderId ColliderId;
    
    [LabelText("飞行物的id")]
    public NP_BlackBoardRelationData<long> FlyObjUnit = new ();
    
    [LabelText("是否在飞行途中能打中物体")]
    public BlackboardOrValue_Bool IsFlyingTrigger;
    
    [LabelText("射中的目标")]
    public NP_BlackBoardRelationData<long> TriggeredUnit = new ();

    [LabelText("是否跟随目标")]
    public BlackboardOrValue_Bool IsFollowTarget;

    [LabelText("跟随的目标")]
    public NP_BlackBoardRelationData<long> FollowTarget = new();

    [LabelText("相对于角色的发射方向")]
    public BlackboardOrValue_Vector3 FlyDir;

    
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