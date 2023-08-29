using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("创建单一飞行物", TitleAlignment = TitleAlignments.Centered)]
public class CreateSingleFlyAction : SkillBaseAction
{
    [LabelText("飞行物prefab")]
    public string PrefabPath;

    public NormalDefaultColliderNode NormalDefaultColliderNode = new();

    [LabelText("飞行物出生点")]
    public string HangPoint;

    [LabelText("飞行速度")]
    public float Speed;

    [LabelText("是否跟随目标")]
    [HideIf(nameof(IsFlyToTarget))]
    public bool IsFollowTarget = true;

    [HideIf(nameof(IsFollowTarget))]
    [LabelText("是否朝某个物体发射")]
    public bool IsFlyToTarget;

    [LabelText("朝向或跟随的物体")]
    [ShowIf("@IsFlyToTarget || IsFollowTarget")]
    public BlackboardOrValue_Long FlyToTarget = new();

    [LabelText("发射方向")]
    [ShowIf("@!IsFlyToTarget && !IsFollowTarget")]
    public Vector2 FlyDir;

    [LabelText("飞行的距离")]
    [HideIf(nameof(IsFollowTarget))]
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
                NormalDefaultColliderNode.ToColliderData(BelongtoRuntimeTree.GetBlackboard()), createAsync);
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