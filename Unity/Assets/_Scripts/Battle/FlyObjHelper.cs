using Framework;
using NPBehave;
using Unity.Mathematics;
using UnityEngine;

public static class FlyObjHelper
{
    public static async void CreateSingleFlyObj(NP_CreateSingleFlyAction action, NP_RuntimeTree runtimeTree,
        DefaultColliderData colliderData)
    {
        Scene scene = action.BelongToUnit.DomainScene();
        Blackboard blackboard = runtimeTree.GetBlackboard();
        string prefabPath = action.PrefabPath.GetValue(blackboard);
        string bornPath = action.HangPoint.GetValue(blackboard);
        float speed = action.Speed.GetValue(blackboard);
        bool isFollowTarget = action.IsFollowTarget.GetValue(blackboard);
        bool isFlyToTarget = action.IsFlyToTarget.GetValue(blackboard);
        Transform rootTrans = runtimeTree.BelongToUnit.GetComponent<GameObjectComponent>().GameObject.transform;
        if (!string.IsNullOrEmpty(bornPath))
        {
            rootTrans = rootTrans.Find(bornPath);
        }

        var objUnit = UnitFactory.CreateUnit(scene, 0);
        var selfTrans =
            await ResComponent.Instance.InstantiateAsync(prefabPath, rootTrans);
        objUnit.AddComponent<MoveComponent>();
        objUnit.AddComponent<GameObjectComponent>().GameObject = selfTrans;
        UnitFactory.CreateDefaultColliderUnit(runtimeTree.DomainScene(), selfTrans.gameObject, objUnit.Id, 0, false,
            colliderData);
        if (isFollowTarget)
        {
            objUnit.AddComponent<FollowTargetComponent>()
                .Follow(action.FollowTarget.GetBlackBoardValue(blackboard), 0.1f);
        }
        else
        {
            Vector3 dir;
            if (isFlyToTarget)
            {
                var targetUnit = scene.GetComponent<UnitComponent>().Get(action.FlyToTarget.GetValue(blackboard));
                dir = math.normalize(targetUnit.Position - runtimeTree.BelongToUnit.Position);
            }
            else
            {
                dir = (action.FlyDir.GetValue(blackboard) - (Vector3)runtimeTree.BelongToUnit.Position).normalized;
            }

            objUnit.GetComponent<MoveComponent>().MoveTo(dir * action.FlyDis.GetValue(blackboard), speed);
        }
    }
}