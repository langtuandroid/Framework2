using System;
using Framework;
using Sirenix.OdinInspector;
using Unity.Mathematics;

[Title("创建单一飞行物", TitleAlignment = TitleAlignments.Centered)]
public class NP_CreateSingleFlyAction : NP_ClassForStoreAction
{
    [LabelText("飞行物prefab")]
    public BlackboardOrValue_String PrefabPath = new BlackboardOrValue_String();

    public DefaultColliderNode DefaultColliderNode = new DefaultColliderNode();

    [LabelText("飞行物出生点")]
    public BlackboardOrValue_String HangPoint = new BlackboardOrValue_String();

    [LabelText("飞行速度")]
    public BlackboardOrValue_Float Speed = new BlackboardOrValue_Float(1);
    
    [LabelText("飞行物的id")]
    public NP_BlackBoardRelationData<long> FlyObjUnit = new ();
    
    [LabelText("射中的目标")]
    public NP_BlackBoardRelationData<long> TriggeredUnit = new ();

    [LabelText("是否跟随目标")]
    public BlackboardOrValue_Bool IsFollowTarget = new();

    [LabelText("跟随的目标")]
    [ShowIf("@IsFollowTarget.GetEditorValue()")]
    public NP_BlackBoardRelationData<long> FollowTarget = new();
    
    [LabelText("是否朝某个物体发射")]
    public BlackboardOrValue_Bool IsFlyToTarget = new();

    [LabelText("发射朝向的物体")]
    [ShowIf("@IsFlyToTarget.GetEditorValue()")]
    public BlackboardOrValue_Long FlyToTarget = new();

    [LabelText("发射方向")]
    [HideIf("@IsFlyToTarget.GetEditorValue()")]
    public BlackboardOrValue_Vector3 FlyDir = new();

    [LabelText("飞行的距离")]
    public BlackboardOrValue_Float FlyDis = new();

    public override Action GetActionToBeDone()
    {
        this.Action = this.Create;
        return this.Action;
    }

    private void Create()
    {
        FlyObjHelper.CreateSingleFlyObj(this, BelongtoRuntimeTree, DefaultColliderNode.ToColliderData(BelongtoRuntimeTree.GetBlackboard()));
    }
}