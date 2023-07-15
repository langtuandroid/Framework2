using Framework;

public class ColliderUserData : Entity, IAwakeSystem<Unit,object>
{
    public Unit Unit;
    public object UserData;

    public void Awake(Unit a, object b)
    {
        Unit = a;
        UserData = b;
    }
}

public class ColliderArgs : IReference
{
    public string CollisionHandlerName;
    public Unit BelongToUnit;
    public object UserData;

    public void Clear()
    {
        CollisionHandlerName = default;
        BelongToUnit = default;
        UserData = default;
    }
}

public class DefaultColliderData
{
    public long BelongSkillRootId;
    public RoleTag RoleTag;
    public RoleCast RoleCast;
    public string IsHitBlackboardKey;
    public string HitUnitListBlackboardKey;
    public string HitUnitBlackboardKey;

    public DefaultColliderData(long belongSkillRootId, RoleTag roleTag, RoleCast roleCast, string isHitBlackboardKey,string hitUnitBlackboardKey, string hitUnitListBlackboardKey)
    {
        BelongSkillRootId = belongSkillRootId;
        RoleTag = roleTag;
        RoleCast = roleCast;
        HitUnitBlackboardKey = hitUnitBlackboardKey;
        HitUnitListBlackboardKey = hitUnitListBlackboardKey;
        IsHitBlackboardKey = isHitBlackboardKey;
    }
}