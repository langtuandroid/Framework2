using ET;
using Framework;

[Invoke(BattleTimerType.ResurrectionTimer)]
public class ResurrectionTimer : ATimer<DeadComponent>
{
    protected override void Run(DeadComponent self)
    {
        self.GetParent<Unit>()?.RemoveComponent<DeadComponent>();
    }
}

/// <summary>
/// 添加此组件代表Unit已死亡，一些其他组件逻辑将暂时失灵
/// 相应的，移除此组件，其他组件的逻辑将恢复正常
/// </summary>
public class DeadComponent : Entity, IAwakeSystem<long>, IDestroySystem
{
    /// <summary>
    /// 复活时长，在经过这个时间之后DeadComponent将会被移除, 当前默认10s
    /// </summary>
    public long ResurrectionTime = 10000;

    public long DeadTimerId;

    public void Awake(long resurrectionTime)
    {
        ResurrectionTime = resurrectionTime;

        Unit unit = GetParent<Unit>();

        // 休眠刚体，不再会产生碰撞
        unit.GetComponent<B2S_ColliderComponent>().Body.IsAwake = false;

        DeadTimerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ClientNow() + ResurrectionTime,
            BattleTimerType.ResurrectionTimer, this);
    }

    public void OnDestroy()
    {
        Unit unit = GetParent<Unit>();

        if (!(unit.GetComponent<B2S_ColliderComponent>().Body is null))
        {
            unit.GetComponent<B2S_ColliderComponent>().Body.IsAwake = true;
        }

        TimerComponent.Instance.Remove(ref DeadTimerId);
    }
}