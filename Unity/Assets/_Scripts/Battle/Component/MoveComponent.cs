using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class MoveComponent : Entity, IAwakeSystem, IDestroySystem
{
    public Vector3 PreTarget
    {
        get { return this.Targets[this.NextPointIndex - 1]; }
    }

    public Vector3 NextTarget
    {
        get { return this.Targets[this.NextPointIndex]; }
    }

    // 开启一次移动协程的时间点
    public long BeginTime;

    /// <summary>
    /// 两点之间寻路的累加计时器
    /// </summary>
    public long AccumulateTime;

    /// <summary>
    /// 目标的范围
    /// </summary>
    public float TargetRange = 0;

    // 每个点的开始移动的时间点
    public long StartTime { get; set; }

    // 开启移动时的Unit的位置
    public Vector3 StartPos;

    private long needTime;

    public long NeedTime
    {
        get { return this.needTime; }
        set { this.needTime = value; }
    }

    public float Speed; // m/s

    public Action<bool> Callback;

    public List<Vector3> Targets = new List<Vector3>();

    public Vector3 FinalTarget
    {
        get
        {
            if (this.Targets.Count > 0)
            {
                return this.Targets[this.Targets.Count - 1];
            }

            return Vector3.zero;
        }
    }

    public bool ShouldMove = false;
    public bool StartMoveCurrentFrame = false;

    /// <summary>
    /// 下一个路径点的索引值
    /// </summary>
    public int NextPointIndex;

    public int TurnTime;

    public bool IsTurnHorizontal;

    public Quaternion From;

    public Quaternion To;


    public void Awake()
    {
        StartTime = 0;
        StartPos = Vector3.zero;
        NeedTime = 0;
        Callback = null;
        Targets.Clear();
        Speed = 0;
        NextPointIndex = 0;
        TurnTime = 0;
    }

    public bool IsArrived()
    {
        return Targets.Count == 0;
    }

    public bool ChangeSpeed(float speed)
    {
        if (IsArrived())
        {
            return false;
        }

        if (speed < 0.0001)
        {
            return false;
        }

        Unit unit = GetParent<Unit>();
        using (RecyclableList<Vector3> path = RecyclableList<Vector3>.Create())
        {
            path.Add(unit.Position); // 第一个是Unit的pos
            for (int i = NextPointIndex; i < Targets.Count; ++i)
            {
                path.Add(Targets[i]);
            }

            Stop();
            MoveToAsync(path, speed).Coroutine();
        }

        return true;
    }

    public async ETTask<bool> MoveToAsync(List<Vector3> target, float speed,
        int turnTime = 100, float targetRange = 0, ETCancellationToken cancellationToken = null)
    {
        Stop();
        TargetRange = targetRange;

        foreach (Vector3 v in target)
        {
            Targets.Add(v);
        }

        IsTurnHorizontal = true;
        TurnTime = turnTime;
        Speed = speed;
        ETTask<bool> tcs = ETTask<bool>.Create(true);
        Callback = (ret) => { tcs.SetResult(ret); };

        StartMove();

        void CancelAction()
        {
            Stop();
        }

        bool moveRet;
        try
        {
            cancellationToken?.Add(CancelAction);
            moveRet = await tcs;
        }
        finally
        {
            cancellationToken?.Remove(CancelAction);
        }

        return moveRet;
    }

    public void MoveForward(long deltaTime, bool needCancel)
    {
        Unit unit = GetParent<Unit>();

        long moveTime = AccumulateTime += deltaTime;

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
                    Vector3 newPos = Vector3.Lerp(StartPos, NextTarget, amount);
                    unit.Position = newPos;
                }

                // 计算方向插值
                if (TurnTime > 0)
                {
                    amount = moveTime * 1f / TurnTime;
                    Quaternion q = Quaternion.Slerp(From, To, amount);
                    unit.Rotation = q;
                }
            }

            moveTime -= NeedTime;

            // 如果抵达了目标范围，强行让客户端停止
            if (Vector3.Distance(unit.Position, FinalTarget) - TargetRange <= 0.0001f)
            {
                unit.Rotation = To;

                Action<bool> callback = Callback;
                Callback = null;

                Clear();
                callback?.Invoke(true);
                return;
            }

            // 表示这个点还没走完，等下一帧再来
            if (moveTime < 0)
            {
                return;
            }

            // 如果是最后一个点
            if (NextPointIndex >= Targets.Count - 1)
            {
                unit.Position = NextTarget;
                unit.Rotation = To;

                Action<bool> callback = Callback;
                Callback = null;

                Clear();
                callback?.Invoke(!needCancel);
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
        ShouldMove = true;
    }

    private void SetNextTarget()
    {
        Unit unit = GetParent<Unit>();

        ++NextPointIndex;

        // 时间计算用服务端的位置, 但是移动要用客户端的位置来插值
        Vector3 v = GetFaceV();
        float distance = v.magnitude;

        // 插值的起始点要以unit的真实位置来算
        StartPos = unit.Position;

        AccumulateTime = 0;
        StartTime += NeedTime;

        NeedTime = (long)(distance / Speed * 1000);


        if (TurnTime > 0)
        {
            // 要用unit的位置
            Vector3 faceV = GetFaceV();
            if (faceV.sqrMagnitude < 0.0001f)
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
                To = Quaternion.LookRotation(faceV, Vector3.up);
            }

            return;
        }

        if (TurnTime == 0) // turn time == 0 立即转向
        {
            Vector3 faceV = GetFaceV();
            if (IsTurnHorizontal)
            {
                faceV.y = 0;
            }

            if (Math.Abs(faceV.x) > 0.01 || Math.Abs(faceV.z) > 0.01)
            {
                To = Quaternion.LookRotation(faceV, Vector3.up);
                unit.Rotation = To;
            }
        }
    }

    private Vector3 GetFaceV()
    {
        return NextTarget - PreTarget;
    }

    public bool FlashTo(Vector3 target)
    {
        Unit unit = GetParent<Unit>();
        unit.Position = target;
        return true;
    }

    public bool MoveTo(Vector3 target, float speed, int turnTime = 0,
        bool isTurnHorizontal = false)
    {
        if (speed < 0.001)
        {
            Log.Error($"speed is 0 {GetParent<Unit>().Id} {speed}");
            return false;
        }

        Stop();

        IsTurnHorizontal = isTurnHorizontal;
        TurnTime = turnTime;
        Targets.Add(GetParent<Unit>().Position);
        Targets.Add(target);
        Speed = speed;

        StartMove();
        return true;
    }

    public bool MoveTo(List<Vector3> target, float speed, int turnTime = 0)
    {
        if (target.Count == 0)
        {
            return true;
        }

        if (Math.Abs(speed) < 0.001)
        {
            Log.Error($"speed is 0 {GetParent<Unit>().Id}");
            return false;
        }

        Stop();

        foreach (Vector3 v in target)
        {
            Targets.Add(v);
        }

        IsTurnHorizontal = true;
        TurnTime = turnTime;
        Speed = speed;

        StartMove();

        return true;
    }

    public void Stop(bool result = false)
    {
        Clear(result);
    }

    public void Clear(bool result = false)
    {
        StartTime = 0;
        StartPos = Vector3.zero;
        BeginTime = 0;
        NeedTime = 0;
        AccumulateTime = 0;
        Targets.Clear();
        Speed = 0;
        NextPointIndex = 0;
        TurnTime = 0;
        IsTurnHorizontal = false;
        ShouldMove = false;

        if (Callback != null)
        {
            Action<bool> callback = Callback;
            Callback = null;
            callback.Invoke(result);
        }
    }


    public void OnDestroy()
    {
        Clear();
    }
}