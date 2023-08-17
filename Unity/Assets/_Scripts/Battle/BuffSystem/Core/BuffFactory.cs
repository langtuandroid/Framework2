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
        /// <param name="npDataSupportor">Buff数据归属的数据块</param>
        /// <param name="buffNodeId">Buff节点的Id</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <returns></returns>
        public static IBuffSystem AcquireBuff(NP_DataSupportor npDataSupportor, long buffNodeId, Unit theUnitFrom,
            Unit theUnitBelongTo,
            NP_RuntimeTree theSkillCanvasBelongTo)
        {
            return AcquireBuff((npDataSupportor.BuffNodeDataDic[buffNodeId] as NormalBuffNodeData)?.BuffData,
                buffNodeId,
                theUnitFrom, theUnitBelongTo,
                theSkillCanvasBelongTo);
        }

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="buffDataBase">Buff数据</param>
        /// <param name="buffNodeId"></param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <param name="theSkillCanvasBelongTo"></param>
        /// <returns></returns>
        public static IBuffSystem AcquireBuff(BuffDataBase buffDataBase, long buffNodeId, Unit theUnitFrom,
            Unit theUnitBelongTo,
            NP_RuntimeTree theSkillCanvasBelongTo)
        {
            IBuffSystem resultBuff = ReferencePool.Allocate(AllBuffSystemTypes[buffDataBase.GetType()]) as IBuffSystem;
            resultBuff.BelongtoRuntimeTree = theSkillCanvasBelongTo;
            resultBuff.BuffNodeId = buffNodeId;
            resultBuff.Init(buffDataBase, theUnitFrom, theUnitBelongTo, TimeInfo.Instance.ClientNow());
            return resultBuff;
        }
        
        public static IBuffSystem AcquireBuff(long rootNodeId, long buffNodeId, Unit theUnitFrom, Unit theUnitBelongTo,
            NP_RuntimeTree theSkillCanvasBelongTo)
        {
            return AcquireBuff(
                (theUnitFrom.DomainScene().GetComponent<NP_TreeDataRepositoryComponent>().GetNPTreeData(rootNodeId)
                    .BuffNodeDataDic[buffNodeId] as NormalBuffNodeData)?.BuffData, buffNodeId,
                theUnitFrom, theUnitBelongTo, theSkillCanvasBelongTo);
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