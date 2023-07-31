using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("创建单一飞行物", TitleAlignment = TitleAlignments.Centered)]
public class NP_CreateSingleFlyAction : NP_ClassForStoreAction
{
    [BoxGroup("/传回的数据")]
    [LabelText("飞行物的id")]
    public NP_BlackBoardRelationData<long> FlyObjUnitKey = new();

    [BoxGroup("/传回的数据")]
    [LabelText("飞行的终点")]
    public NP_BlackBoardRelationData<Vector3> EndPointKey = new();

    [LabelText("飞行物prefab")]
    public BlackboardOrValue_String PrefabPath = new BlackboardOrValue_String();

    public DefaultColliderNode DefaultColliderNode = new DefaultColliderNode();

    [LabelText("飞行物出生点")]
    public BlackboardOrValue_String HangPoint = new BlackboardOrValue_String();

    [LabelText("飞行速度")]
    public BlackboardOrValue_Float Speed = new BlackboardOrValue_Float(1);
    
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
        return Create;
    }

    private void Create()
    {
        FlyObjHelper.CreateSingleFlyObj(this, BelongtoRuntimeTree,
            DefaultColliderNode.ToColliderData(BelongtoRuntimeTree.GetBlackboard()));
    }
}