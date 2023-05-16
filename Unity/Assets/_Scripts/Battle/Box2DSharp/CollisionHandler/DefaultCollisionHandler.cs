using System.Collections.Generic;
using Framework;
using UnityEngine;

public class DefaultCollisionHandler: AB2D_CollisionHandler
{
    public override void HandleCollisionStart(ColliderUserData a, ColliderUserData b)
    {
        B2D_ColliderComponent aColliderComponent = a.Unit.GetComponent<B2D_ColliderComponent>();
        B2D_RoleCastComponent aRole = aColliderComponent.BelongToUnit.GetComponent<B2D_RoleCastComponent>();
        DefaultColliderData aColliderData = a.UserData as DefaultColliderData;

        B2D_ColliderComponent bColliderComponent = b.Unit.GetComponent<B2D_ColliderComponent>();
        B2D_RoleCastComponent bRole = bColliderComponent.BelongToUnit.GetComponent<B2D_RoleCastComponent>();
        DefaultColliderData bColliderData = a.UserData as DefaultColliderData;

        RoleCast roleCast = aRole.GetRoleCastToTarget(bColliderComponent.BelongToUnit);

            Log.Msg(aColliderComponent.BelongToUnit.GetComponent<GameObjectComponent>().GameObject.name, "碰到了",
                bColliderComponent.BelongToUnit.GetComponent<GameObjectComponent>().GameObject.name);
        if (aColliderData.RoleCast == roleCast && aColliderData.RoleTag.HasFlag(bRole.RoleTag))
        {
            BroadcastCollider(aColliderComponent, bColliderComponent, aColliderData);
        }
    }

    private void BroadcastCollider(B2D_ColliderComponent aColliderComponent, B2D_ColliderComponent bColliderComponent,
        DefaultColliderData aColliderData)
    {
        List<NP_RuntimeTree> targetSkillCanvas = aColliderComponent.GetParent<Unit>()
            .GetComponent<SkillCanvasManagerComponent>()
            .GetSkillCanvas(aColliderData.BelongSkillConfigId);

        foreach (var skillCanvas in targetSkillCanvas)
        {
            if (!string.IsNullOrEmpty(aColliderData.HitUnitsBlackboardKey))
            {
                skillCanvas.GetBlackboard().Get<List<long>>(aColliderData.HitUnitsBlackboardKey)
                    ?.Add(bColliderComponent.BelongToUnit.Id);
            }

            if (!string.IsNullOrEmpty(aColliderData.IsHitBlackboardKey))
            {
                skillCanvas.GetBlackboard().Set(aColliderData.IsHitBlackboardKey, true);
            }
        }
    }

    public override void HandleCollisionStay(ColliderUserData a, ColliderUserData b)
    {
    }

    public override void HandleCollisionEnd(ColliderUserData a, ColliderUserData b)
    {
    }
}