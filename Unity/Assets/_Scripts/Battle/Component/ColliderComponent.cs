using Framework;

public class ColliderComponent : Entity ,IAwakeSystem<Unit>, IStartSystem
{
    private TriggerListener triggerListener;
    
    /// <summary>
    /// 所归属的Unit，也就是产出碰撞体的Unit，
    /// 比如诺克放一个Q，那么BelongUnit就是诺克
    /// 需要注意的是，如果这个碰撞体需要同步位置，同步目标是Parent，而不是这个BelongToUnit
    /// </summary>
    public Unit BelongToUnit;
    
    public void Awake(Unit unit)
    {
        BelongToUnit = unit;
    }
    
    public void Start()
    {
        triggerListener = parent.GetComponent<GameObjectComponent>().GameObject.GetOrAddComponent<TriggerListener>();
    }

}

public class ColliderData
{
    public long BelongToUnit;
    public long BelongSkillRootId;
    public RoleTag RoleTag;
    public RoleCast RoleCast;
    public string IsHitBlackboardKey;
    public string HitUnitsBlackboardKey;

    public ColliderData(long belongSkillRootId, RoleTag roleTag, RoleCast roleCast, string hitUnitsBlackboardKey, string isHitBlackboardKey)
    {
        BelongSkillRootId = belongSkillRootId;
        RoleTag = roleTag;
        RoleCast = roleCast;
        HitUnitsBlackboardKey = hitUnitsBlackboardKey;
        IsHitBlackboardKey = isHitBlackboardKey;
    }
}
