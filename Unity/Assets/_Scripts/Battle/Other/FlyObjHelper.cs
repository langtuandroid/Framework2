using System.Collections.Generic;
using Framework;
using NPBehave;
using Unity.Mathematics;
using UnityEngine;
using Root = NPBehave.Root;

public static class FlyObjHelper
{

    public static async void CreateSingleFlyObj(NP_CreateSingleFlyAction action, NP_RuntimeTree runtimeTree,
        NormalDefaultColliderData colliderData, IPromise promise)
    {
        Scene scene = action.BelongToUnit.DomainScene();
        string prefabPath = action.PrefabPath;
        string bornPath = action.HangPoint;
        float speed = action.Speed;
        bool isFlyToTarget = action.IsFlyToTarget;
        Transform rootTrans = runtimeTree.BelongToUnit.GetComponent<GameObjectComponent>().GameObject.transform;
        if (!string.IsNullOrEmpty(bornPath))
        {
            rootTrans = rootTrans.Find(bornPath);
        }

        Unit objUnit = UnitFactory.CreateUnit(scene);
        GameObject selfTrans =
            await ResComponent.Instance.InstantiateAsync(prefabPath);
        objUnit.Position = rootTrans.position;
        selfTrans.transform.position = rootTrans.position;
        objUnit.AddComponent<MoveComponent>();
        objUnit.AddComponent<GameObjectComponent, bool, bool>(false, true).GameObject = selfTrans;
        objUnit.AddComponent<FlyObjCollideComponent, NormalDefaultColliderData>(colliderData);
        // 障碍物和飞行物同时绑定一个GameObject会导致GoConnectedUnitId出错
        UnitFactory.CreateNormalDefaultColliderUnit(runtimeTree.DomainScene(),
            selfTrans.GetComponentInChildren<Collider>().gameObject,
            runtimeTree.BelongToUnit.Id, -1, false,
            colliderData);
        if (action.IsFollowTarget)
        {
            objUnit.AddComponent<NumericComponent>().Set(NumericType.SpeedBase, speed);
            objUnit.AddComponent<FollowTargetComponent>()
                .Follow(action.FlyToTarget.GetValue(runtimeTree.GetBlackboard()), 0.1f);
            promise.SetResult();
            return;
        }
        float3 endPoint;
        if (isFlyToTarget)
        {
            Unit targetUnit = scene.GetComponent<UnitComponent>()
                .Get(action.FlyToTarget.GetValue(runtimeTree.GetBlackboard()));
            endPoint = math.normalize(targetUnit.Position - runtimeTree.BelongToUnit.Position);
        }
        else
        {
            Vector3 dir = new(action.FlyDir.x, 0, action.FlyDir.y);
            endPoint = dir.normalized;
        }

        endPoint *= action.FlyDis;
        endPoint += runtimeTree.BelongToUnit.Position;
        objUnit.GetComponent<MoveComponent>().MoveTo(endPoint, speed);
        promise.SetResult();
    }

}