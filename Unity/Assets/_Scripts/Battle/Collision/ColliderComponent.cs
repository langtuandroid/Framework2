using Framework;
using UnityEngine;

/// <summary>
/// 一个碰撞体Component,包含一个碰撞实例所有信息，直接挂载到碰撞体Unit上
/// 比如诺手Q技能碰撞体UnitA，那么这个B2D_ColliderComponent的Entity就是UnitA，而其中的BelongToUnit就是诺手
/// </summary>
public class ColliderComponent : Entity, IAwakeSystem<ColliderArgs>, IStartSystem, IUpdateSystem, IDestroySystem
{
    /// <summary>
    /// 碰撞处理者名称
    /// </summary>
    public string CollisionHandlerName;

    private TriggerListener triggerListener;

    /// <summary>
    /// 所归属的Unit，也就是产出碰撞体的Unit，
    /// 比如诺克放一个Q，那么BelongUnit就是诺克
    /// 需要注意的是，如果这个碰撞体需要同步位置，同步目标是Parent，而不是这个BelongToUnit
    /// </summary>
    public Unit BelongToUnit;

    private object userData;

    private Unit selfUnit;

    private ColliderUserData selfColliderUserData;

    public void Awake(ColliderArgs args)
    {
        CollisionHandlerName = args.CollisionHandlerName ?? string.Empty;
        BelongToUnit = args.BelongToUnit;
        userData = args.UserData;
        selfUnit = GetParent<Unit>();
        selfColliderUserData = new ColliderUserData(selfUnit, args.UserData);
    }

    public void Start()
    {
        triggerListener = parent.GetComponent<GameObjectComponent>().GameObject.GetOrAddComponent<TriggerListener>();
        triggerListener.TriggerEnter += OnTriggerEnter;
        triggerListener.TriggerExit += OnTriggerExit;
        triggerListener.TriggerStay += OnTriggerStay;
    }

    public void Update(float deltaTime)
    {
    }

    public void OnDestroy()
    {
    }

    private void OnTriggerEnter(GameObject other)
    {
        GoConnectedUnitId connectedUnitId;
        if((connectedUnitId = other.GetComponent<GoConnectedUnitId>()) == null) return;
        var targetUnit = this.domain.GetComponent<UnitComponent>().Get(connectedUnitId.UnitId);
        if(targetUnit == null) return;
        ColliderUserData targetUserData = ReferencePool.Allocate<ColliderUserData>();
        targetUserData.Unit = targetUnit;
        CollisionHandlerCollector.Instance.HandleCollisionStart(selfColliderUserData, targetUserData);
        ReferencePool.Free(targetUserData);
    }

    private void OnTriggerExit(GameObject other)
    {
    }

    private void OnTriggerStay(GameObject other)
    {
    }

}