using System.Collections.Generic;
using Framework;
using NPBehave;

public class DefaultCollisionHandler: ACollisionHandler
{
    public override void HandleCollisionStart(ColliderUserData a, ColliderUserData b)
    {
        DefaultColliderData aColliderData = a.UserData as DefaultColliderData;

        ColliderComponent aColliderComponent = a.Unit.GetComponent<ColliderComponent>();
        RoleCastComponent aRole = aColliderComponent.BelongToUnit.GetComponent<RoleCastComponent>();

        ColliderComponent bColliderComponent = b.Unit.GetComponent<ColliderComponent>();
        RoleCastComponent bRole = bColliderComponent.BelongToUnit.GetComponent<RoleCastComponent>();

        RoleCast roleCast = aRole.GetRoleCastToTarget(bColliderComponent.BelongToUnit);

        Log.Msg(aColliderComponent.BelongToUnit.GetComponent<GameObjectComponent>().GameObject.name, "碰到了",
            bColliderComponent.BelongToUnit.GetComponent<GameObjectComponent>().GameObject.name);
        if (aColliderData.OnlyTarget != default)
        {
            if (b.Unit.Id != aColliderData.OnlyTarget) return;
            BroadcastCollider(aColliderComponent, bColliderComponent, aColliderData);
        }
        else if (aColliderData.RoleCast == roleCast && aColliderData.RoleTag.Contains(bRole.RoleTag))
        {
            BroadcastCollider(aColliderComponent, bColliderComponent, aColliderData);
        }
    }

    private void BroadcastCollider(ColliderComponent aColliderComponent, ColliderComponent bColliderComponent,
        DefaultColliderData aColliderData)
    {
        Log.Msg(aColliderComponent.BelongToUnit, "撞到了", bColliderComponent.BelongToUnit);

        Blackboard blackboard = aColliderData.Blackboard;
        
        if (!string.IsNullOrEmpty(aColliderData.HitUnitListBlackboardKey))
        {
            blackboard.Get<List<long>>(aColliderData.HitUnitListBlackboardKey)
                .Add(bColliderComponent.BelongToUnit.Id);
        }
        
        if (!string.IsNullOrEmpty(aColliderData.HitUnitBlackboardKey))
        {
            blackboard.Set(aColliderData.HitUnitBlackboardKey, bColliderComponent.BelongToUnit.Id);
        }

        if (!string.IsNullOrEmpty(aColliderData.IsHitBlackboardKey))
        {
            blackboard.Set(aColliderData.IsHitBlackboardKey, true);
        }
    }

    public override void HandleCollisionStay(ColliderUserData a, ColliderUserData b)
    {
    }

    public override void HandleCollisionEnd(ColliderUserData a, ColliderUserData b)
    {
    }
}