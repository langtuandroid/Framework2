using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// Buff工厂
    /// </summary>
    public static class BuffFactory
    {
        /// <summary>
        /// 记录所有BuffSystem类型，用于运行时创建对应的BuffSystem
        /// </summary>
        public static Dictionary<Type, Type> AllBuffSystemTypes = new Dictionary<Type, Type>()
        {
            {typeof(ChangePropertyBuffData), typeof(ChangePropertyBuffSystem)},
            {typeof(RefreshTargetBuffTimeBuffData), typeof(RefreshTargetBuffTimeBuffSystem)},
            {typeof(FlashDamageBuffData), typeof(FlashDamageBuffSystem)},
            {typeof(SustainDamageBuffData), typeof(SustainDamageBuffSystem)},
            {typeof(TreatmentBuffData), typeof(TreatmentBuffSystem)},
        };

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="buffDataBase">Buff数据</param>
        /// <param name="buffNodeId"></param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <param name="theSkillCanvasBelongTo"></param>
        /// <returns></returns>
        public static IBuffSystem AcquireBuff(BuffDataBase buffDataBase, Unit theUnitFrom,
            Unit theUnitBelongTo)
        {
            IBuffSystem resultBuff = ReferencePool.Allocate(AllBuffSystemTypes[buffDataBase.GetType()]) as IBuffSystem;
            resultBuff.Init(buffDataBase, theUnitFrom, theUnitBelongTo, TimeInfo.Instance.ClientNow());
            return resultBuff;
        }
        
        /// <summary>
        /// 回收一个Buff
        /// </summary>
        /// <param name="aBuffSystemBase"></param>
        public static void RecycleBuff<T>(ABuffSystemBase<T> aBuffSystemBase) where T : BuffDataBase
        {
            ReferencePool.Free(aBuffSystemBase);
        }
    }
}