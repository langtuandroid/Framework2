using Framework;

/// <summary>
/// 非行为树里的默认碰撞器
/// </summary>
public class NormalDefaultCollisionHandler : ACollisionHandler
{
    public override void HandleCollisionStart(ColliderUserData a, ColliderUserData b)
    {
        var aColliderData = a.UserData as NormalDefaultColliderData;

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
        NormalDefaultColliderData aColliderData)
    {
        Log.Msg(aColliderComponent.BelongToUnit, "撞到了", bColliderComponent.BelongToUnit);
        aColliderData.Targets.Add(bColliderComponent.BelongToUnit.Id);
        aColliderData.IsHit.Value = true;
    }

    public override void HandleCollisionStay(ColliderUserData a, ColliderUserData b)
    {
    }

    public override void HandleCollisionEnd(ColliderUserData a, ColliderUserData b)
    {
    }
}