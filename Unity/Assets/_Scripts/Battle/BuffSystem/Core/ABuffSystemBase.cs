﻿

namespace Framework
{

    public abstract class ABuffSystemBase<T> : IBuffSystem, IReference where T : BuffDataBase
    {
        public NP_RuntimeTree BelongtoRuntimeTree { get; set; }
        public BuffState BuffState { get; set; }
        public int CurrentOverlay { get; set; }

        /// <summary>
        /// 持续时间（目标帧）（到达这个帧就会移除）
        /// </summary>
        public float MaxLimitTime { get; set; }

        public BuffDataBase BuffData { get; set; }
        public long BuffNodeId { get; set; }
        public Unit TheUnitFrom { get; set; }
        public Unit TheUnitBelongto { get; set; }

        /// <summary>
        /// 获取自身的BuffData数据，自动转型为T
        /// </summary>
        public T GetBuffDataWithTType => BuffData as T;

        public void Init(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto, long currentTime)
        {
            //设置Buff来源Unit和归属Unit
            this.TheUnitFrom = theUnitFrom;
            this.TheUnitBelongto = theUnitBelongto;
            this.BuffData = buffData as T;

            // 如果没加入成功，说明已有同一个Buff，需要进入对象池
            if (BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, currentTime))
            {
                this.BuffState = BuffState.Waiting;
                OnInit(buffData, theUnitFrom, theUnitBelongto, currentTime);
            }
            else
            {
                ReferencePool.Free(this);
            }
        }

        public void Excute(long currentTime)
        {
            switch (this.BuffData.SustainTime)
            {
                case 0:
                    this.BuffState = BuffState.Finished;
                    break;
                case -1:
                    this.BuffState = BuffState.Forever;
                    break;
                default:
                    this.BuffState = BuffState.Running;
                    break;
            }

            this.OnExecute(currentTime);
        }

        public void Update(long currentTime)
        {
            if (currentTime >= MaxLimitTime && this.BuffState != BuffState.Forever)
            {
                this.BuffState = BuffState.Finished;
            }
            else
            {
                this.OnUpdate(currentTime);
            }
        }

        public void Finished(long currentTime)
        {
            this.OnFinished(currentTime);
        }

        public void Refresh(long currentTime)
        {
            this.OnRefreshed(currentTime);
        }

        /// <summary>
        /// 初始化buff数据
        /// </summary>
        /// <param name="buffData">Buff数据</param>
        /// <param name="theUnitFrom">来自哪个Unit</param>
        /// <param name="theUnitBelongto">寄生于哪个Unit</param>
        /// <param name="currentTime"></param>
        public virtual void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto, long currentTime)
        {
        }

        /// <summary>
        /// Buff触发
        /// </summary>
        public abstract void OnExecute(long currentTime);

        /// <summary>
        /// Buff持续
        /// </summary>
        public virtual void OnUpdate(long currentTime)
        {
        }

        /// <summary>
        /// 重置Buff用
        /// </summary>
        public virtual void OnFinished(long currentTime)
        {
        }

        /// <summary>
        /// 刷新，用于刷新Buff状态
        /// </summary>
        public virtual void OnRefreshed(long currentTime)
        {
        }

        public void Clear()
        {
            BelongtoRuntimeTree = null;
            BuffState = BuffState.Waiting;
            CurrentOverlay = 0;
            MaxLimitTime = 0;
            BuffData = null;
            TheUnitFrom = null;
            TheUnitBelongto = null;
        }
    }
}