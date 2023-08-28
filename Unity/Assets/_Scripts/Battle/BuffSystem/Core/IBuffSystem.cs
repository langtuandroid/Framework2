namespace Framework
{

    public interface IBuffSystem
    {
        /// <summary>
        /// 归属的运行时行为树实例
        /// </summary>
        NP_RuntimeTree BelongtoRuntimeTree { get; set; }

        /// <summary>
        /// Buff当前状态
        /// </summary>
        BuffState BuffState { get; set; }

        /// <summary>
        /// 当前叠加数
        /// </summary>
        int CurrentOverlay { get; set; }

        /// <summary>
        /// 持续时间（目标帧）（到达这个帧就会移除）
        /// </summary>
        float MaxLimitTime { get; set; }

        /// <summary>
        /// Buff数据
        /// </summary>
        BuffDataBase BuffData { get; set; }

        /// <summary>
        /// 来自哪个Unit
        /// </summary>
        Unit TheUnitFrom { get; set; }

        /// <summary>
        /// 寄生于哪个Unit，并不代表当前Buff实际寄居者，需要通过GetBuffTarget来获取，因为它赋值于Buff链起源的地方，具体值取决于那个起源Buff
        /// </summary>
        Unit TheUnitBelongto { get; set; }

        void Init(BuffDataBase buffDataBase, Unit theUnitFrom, Unit theUnitBelongto, long currentTime);

        void Excute(long currentTime);

        void Update(long currentTime);

        void Finished(long currentTime);

        void Refresh(long currentTime);
    }
}