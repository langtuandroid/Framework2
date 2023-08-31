using Framework;
using NPBehave;

public class ColliderUserData : IReference
{
    public Unit Unit;
    public object UserData;

    private ColliderUserData()
    {
    }

    public ColliderUserData(Unit a, object b)
    {
        Unit = a;
        UserData = b;
    }

    public void Clear()
    {
        Unit = null;
        UserData = null;
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
    public Blackboard Blackboard;
    public RoleTag RoleTag;
    public RoleCast RoleCast;
    // 指定打击目标
    public long OnlyTarget;
    public string IsHitBlackboardKey;
    public string HitUnitListBlackboardKey;
    public string HitUnitBlackboardKey;

    public DefaultColliderData(Blackboard blackboard, RoleTag roleTag, RoleCast roleCast, string isHitBlackboardKey,string hitUnitBlackboardKey, string hitUnitListBlackboardKey, long onlyTarget)
    {
        Blackboard = blackboard;
        RoleTag = roleTag;
        RoleCast = roleCast;
        HitUnitBlackboardKey = hitUnitBlackboardKey;
        HitUnitListBlackboardKey = hitUnitListBlackboardKey;
        OnlyTarget = onlyTarget;
        IsHitBlackboardKey = isHitBlackboardKey;
    }
}