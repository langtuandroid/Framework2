using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Unity.Mathematics;
using UnityEngine;

[Invoke(BattleTimerType.MoveTimer)]
public class MoveTimer : ATimer<MoveComponent>
{
    protected override void Run(MoveComponent self)
    {
        try
        {
            self.MoveForward(true);
        }
        catch (Exception e)
        {
            Log.Error($"move timer error: {self.Id}\n{e}");
        }
    }
}

public class MoveComponent : Entity, IAwakeSystem, IDestroySystem
{
    public float3 PreTarget => Targets[N - 1];

    public float3 NextTarget => Targets[N];

    // 开启移动协程的时间
    public long BeginTime;

    // 每个点的开始时间
    public long StartTime { get; set; }

    // 开启移动协程的Unit的位置
    public float3 StartPos;

    public float3 RealPos => Targets[0];

    private long needTime;

    public long NeedTime
    {
        get => needTime;
        set => needTime = value;
    }

    public long MoveTimer;

    public float Speed; // m/s

    public ETTask<bool> tcs;

    public List<float3> Targets = new();

    public float3 FinalTarget => Targets[Targets.Count - 1];

    public int N;

    public int TurnTime;

    public bool IsTurnHorizontal;

    public quaternion From;

    public quaternion To;

    public void Awake()
    {
        StartTime = 0;
        StartPos = float3.zero;
        NeedTime = 0;
        MoveTimer = 0;
        tcs = null;
        Targets.Clear();
        Speed = 0;
        N = 0;
        TurnTime = 0;
    }

    public bool IsArrived
    {
        get => Targets.Count == 0;
    }

    public bool ChangeSpeed(float speed)
    {
        if (IsArrived)
        {
            return false;
        }

        if (speed < 0.0001)
        {
            return false;
        }

        Unit unit = GetParent<Unit>();

        using RecyclableList<float3> path = RecyclableList<float3>.Create();

        MoveForward(false);

        path.Add(unit.Position); // 第一个是Unit的pos
        for (int i = N; i < Targets.Count; ++i)
        {
            path.Add(Targets[i]);
        }

        MoveToAsync(path, speed).Coroutine();
        return true;
    }

    // 该方法不需要用cancelToken的方式取消，因为即使不传入cancelToken，多次调用该方法也要取消之前的移动协程,上层可以stop取消
    public async ETTask<bool> MoveToAsync(List<float3> target, float speed, int turnTime = 100)
    {
        Stop(false);

        foreach (float3 v in target)
        {
            Targets.Add(v);
        }

        IsTurnHorizontal = true;
        TurnTime = turnTime;
        Speed = speed;
        tcs = ETTask<bool>.Create(true);

        StartMove();

        bool moveRet = await tcs;

        return moveRet;
    }

    public void MoveForward(bool ret)
    {
        Unit unit = GetParent<Unit>();

        long timeNow = TimeHelper.ClientNow();
        long moveTime = timeNow - StartTime;

        while (true)
        {
            if (moveTime <= 0)
            {
                return;
            }

            // 计算位置插值
            if (moveTime >= NeedTime)
            {
                unit.Position = NextTarget;
                if (TurnTime > 0)
                {
                    unit.Rotation = To;
                }
            }
            else
            {
                // 计算位置插值
                float amount = moveTime * 1f / NeedTime;
                if (amount > 0)
                {
                    float3 newPos = math.lerp(StartPos, NextTarget, amount);
                    unit.Position = newPos;
                }

                // 计算方向插值
                if (TurnTime > 0)
                {
                    amount = moveTime * 1f / TurnTime;
                    if (amount > 1)
                    {
                        amount = 1f;
                    }

                    quaternion q = math.slerp(From, To, amount);
                    unit.Rotation = q;
                }
            }

            moveTime -= NeedTime;

            // 表示这个点还没走完，等下一帧再来
            if (moveTime < 0)
            {
                return;
            }

            // 到这里说明这个点已经走完

            // 如果是最后一个点
            if (N >= Targets.Count - 1)
            {
                unit.Position = NextTarget;
                unit.Rotation = To;

                MoveFinish(ret);
                return;
            }

            SetNextTarget();
        }
    }

    private void StartMove()
    {
        BeginTime = TimeHelper.ClientNow();
        StartTime = BeginTime;
        SetNextTarget();
        MoveTimer = TimerComponent.Instance.NewFrameTimer(BattleTimerType.MoveTimer, this);
    }

    private void SetNextTarget()
    {
        Unit unit = GetParent<Unit>();

        ++N;

        // 时间计算用服务端的位置, 但是移动要用客户端的位置来插值
        float3 v = GetFaceV();
        float distance = math.length(v);

        // 插值的起始点要以unit的真实位置来算
        StartPos = unit.Position;

        StartTime += NeedTime;

        NeedTime = (long)(distance / Speed * 1000);

        if (TurnTime > 0)
        {
            // 要用unit的位置
            float3 faceV = GetFaceV();
            if (math.lengthsq(faceV) < 0.0001f)
            {
                return;
            }

            From = unit.Rotation;

            if (IsTurnHorizontal)
            {
                faceV.y = 0;
            }

            if (Math.Abs(faceV.x) > 0.01 || Math.Abs(faceV.z) > 0.01)
            {
                To = quaternion.LookRotation(faceV, math.up());
            }

            return;
        }

        if (TurnTime == 0) // turn time == 0 立即转向
        {
            float3 faceV = GetFaceV();
            if (IsTurnHorizontal)
            {
                faceV.y = 0;
            }

            if (Math.Abs(faceV.x) > 0.01 || Math.Abs(faceV.z) > 0.01)
            {
                To = quaternion.LookRotation(faceV, math.up());
                unit.Rotation = To;
            }
        }
    }

    private float3 GetFaceV()
    {
        return NextTarget - PreTarget;
    }

    public bool FlashTo(float3 target)
    {
        Unit unit = GetParent<Unit>();
        unit.Position = target;
        return true;
    }

    // ret: 停止的时候，移动协程的返回值
    public void Stop(bool ret)
    {
        if (Targets.Count > 0)
        {
            MoveForward(ret);
        }

        MoveFinish(ret);
    }

    private void MoveFinish(bool ret)
    {
        if (StartTime == 0)
        {
            return;
        }

        StartTime = 0;
        StartPos = float3.zero;
        BeginTime = 0;
        NeedTime = 0;
        TimerComponent.Instance?.Remove(ref MoveTimer);
        Targets.Clear();
        Speed = 0;
        N = 0;
        TurnTime = 0;
        IsTurnHorizontal = false;

        if (tcs != null)
        {
            ETTask<bool> tcs = this.tcs;
            this.tcs = null;
            tcs.SetResult(ret);
        }
    }

    public bool MoveTo(float3 target, float speed, int turnTime = 100,
        bool isTurnHorizontal = false)
    {
        if (speed < 0.001)
        {
            Log.Error($"speed is 0 {GetParent<Unit>().Id} {speed}");
            return false;
        }

        Stop(false);

        IsTurnHorizontal = isTurnHorizontal;
        TurnTime = turnTime;
        Targets.Add(GetParent<Unit>().Position);
        Targets.Add(target);
        Speed = speed;

        StartMove();
        return true;
    }

    public bool MoveTo(IEnumerable<float3> target, float speed, int turnTime = 100)
    {
        if (Math.Abs(speed) < 0.001)
        {
            Log.Error($"speed is 0 {GetParent<Unit>().Id}");
            return false;
        }

        foreach (float3 v in target)
        {
            Targets.Add(v);
        }

        if (Targets.Count <= 1)
        {
            Targets.Clear();
            return true;
        }

        Stop(false);

        IsTurnHorizontal = true;
        TurnTime = turnTime;
        Speed = speed;

        StartMove();

        return true;
    }

    public void OnDestroy()
    {
        MoveFinish(true);
    }
}