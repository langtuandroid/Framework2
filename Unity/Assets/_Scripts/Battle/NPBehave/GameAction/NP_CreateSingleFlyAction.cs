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
    public string PrefabPath;

    public DefaultColliderNode DefaultColliderNode = new();

    [LabelText("飞行物出生点")]
    public string HangPoint;

    [LabelText("飞行速度")]
    public float Speed;

    [LabelText("是否朝某个物体发射")]
    public bool IsFlyToTarget;

    [LabelText("发射朝向的物体")]
    [ShowIf("IsFlyToTarget")]
    public BlackboardOrValue_Long FlyToTarget = new();

    [LabelText("发射方向")]
    [HideIf("IsFlyToTarget")]
    public Vector2 FlyDir;

    [LabelText("飞行的距离")]
    public float FlyDis;

    private AsyncResult createAsync;

    public override Func<bool, NPBehave.Action.Result> GetFunc2ToBeDone()
    {
        return Create;
    }

    private NPBehave.Action.Result Create(bool isCancel)
    {
        if (isCancel)
        {
            return NPBehave.Action.Result.SUCCESS;
        }

        if (createAsync == null)
        {
            createAsync = AsyncResult.Create();
            FlyObjHelper.CreateSingleFlyObj(this, BelongtoRuntimeTree,
                DefaultColliderNode.ToColliderData(BelongtoRuntimeTree.GetBlackboard()), createAsync);
            return NPBehave.Action.Result.PROGRESS;
        }

        if (!createAsync.IsDone)
        {
            return NPBehave.Action.Result.PROGRESS;
        }

        createAsync.Dispose();
        createAsync = null;
        return NPBehave.Action.Result.SUCCESS;
    }
}