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

        DeadTimerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ClientNow() + ResurrectionTime,
            BattleTimerType.ResurrectionTimer, this);
    }

    public void OnDestroy()
    {
        TimerComponent.Instance.Remove(ref DeadTimerId);
    }
}