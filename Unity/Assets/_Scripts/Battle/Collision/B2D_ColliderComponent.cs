using Framework;

/// <summary>
/// 一个碰撞体Component,包含一个碰撞实例所有信息，直接挂载到碰撞体Unit上
/// 比如诺手Q技能碰撞体UnitA，那么这个B2D_ColliderComponent的Entity就是UnitA，而其中的BelongToUnit就是诺手
/// </summary>
public class B2D_ColliderComponent : Entity, IAwakeSystem<ColliderArgs> , IUpdateSystem, IDestroySystem
{
    /// <summary>
    /// 碰撞处理者名称
    /// </summary>
    public string CollisionHandlerName;
    
    /// <summary>
    /// 所归属的Unit，也就是产出碰撞体的Unit，
    /// 比如诺克放一个Q，那么BelongUnit就是诺克
    /// 需要注意的是，如果这个碰撞体需要同步位置，同步目标是Parent，而不是这个BelongToUnit
    /// </summary>
    public Unit BelongToUnit;
    
    private object userData;

    private Unit selfUnit;

    public void Awake(ColliderArgs args)
    {
        CollisionHandlerName = args.CollisionHandlerName ?? string.Empty;
        BelongToUnit = args.BelongToUnit;
        userData = args.UserData;
        selfUnit = GetParent<Unit>();
    }
    
    public void Update(float deltaTime)
    {
    }

    public void OnDestroy()
    {
    }

}
