using System.Collections.Generic;
using Framework;
using UnityEngine;

public class DefaultCollisionHandler: AB2D_CollisionHandler
{
    public override void HandleCollisionStart(ColliderUserData a, ColliderUserData b)
    {
        B2D_ColliderComponent aColliderComponent = a.Unit.GetComponent<B2D_ColliderComponent>();
        B2D_RoleCastComponent aRole = aColliderComponent.BelongToUnit.GetComponent<B2D_RoleCastComponent>();

        B2D_ColliderComponent bColliderComponent = b.Unit.GetComponent<B2D_ColliderComponent>();
        B2D_RoleCastComponent bRole = bColliderComponent.BelongToUnit.GetComponent<B2D_RoleCastComponent>();

        RoleCast roleCast = aRole.GetRoleCastToTarget(bColliderComponent.BelongToUnit);

            Log.Msg(aColliderComponent.BelongToUnit.GetComponent<GameObjectComponent>().GameObject.name, "碰到了",
                bColliderComponent.BelongToUnit.GetComponent<GameObjectComponent>().GameObject.name);
        if (a.DefaultColliderData.RoleCast == roleCast && a.DefaultColliderData.RoleTag.HasFlag(bRole.RoleTag))
        {
            BroadcastCollider(aColliderComponent, bColliderComponent, a);
        }
    }

    private void BroadcastCollider(B2D_ColliderComponent aColliderComponent, B2D_ColliderComponent bColliderComponent,
        ColliderUserData a)
    {
        List<NP_RuntimeTree> targetSkillCanvas = aColliderComponent.GetParent<Unit>()
            .GetComponent<SkillCanvasManagerComponent>()
            .GetSkillCanvas(a.DefaultColliderData.BelongSkillConfigId);

        foreach (var skillCanvas in targetSkillCanvas)
        {
            if (!string.IsNullOrEmpty(a.DefaultColliderData.HitUnitsBlackboardKey))
            {
                skillCanvas.GetBlackboard().Get<List<long>>(a.DefaultColliderData.HitUnitsBlackboardKey)
                    ?.Add(bColliderComponent.BelongToUnit.Id);
            }

            if (!string.IsNullOrEmpty(a.DefaultColliderData.IsHitBlackboardKey))
            {
                skillCanvas.GetBlackboard().Set(a.DefaultColliderData.IsHitBlackboardKey, true);
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