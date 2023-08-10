using Framework;
using NPBehave;
using Unity.Mathematics;
using UnityEngine;

public static class FlyObjHelper
{
    public static async void CreateSingleFlyObj(NP_CreateSingleFlyAction action, NP_RuntimeTree runtimeTree,
        DefaultColliderData colliderData, IPromise promise)
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

        Unit objUnit = UnitFactory.CreateUnit(scene, 0);
        action.FlyObjUnitKey.SetBlackBoardValue(runtimeTree.GetBlackboard(), objUnit.Id);
        GameObject selfTrans =
            await ResComponent.Instance.InstantiateAsync(prefabPath);
        objUnit.Position = rootTrans.position;
        objUnit.AddComponent<MoveComponent>();
        objUnit.AddComponent<GameObjectComponent>().GameObject = selfTrans;
        // 障碍物和飞行物同时绑定一个GameObject会导致GoConnectedUnitId出错
        // UnitFactory.CreateDefaultColliderUnit(runtimeTree.DomainScene(), selfTrans.gameObject,
        //     runtimeTree.BelongToUnit.Id, 0, false,
        //     colliderData);
        Vector3 endPoint;
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
        action.EndPointKey.SetBlackBoardValue(runtimeTree.GetBlackboard(), endPoint);

        objUnit.GetComponent<MoveComponent>().MoveTo(endPoint, speed);
        promise.SetResult();
    }
}