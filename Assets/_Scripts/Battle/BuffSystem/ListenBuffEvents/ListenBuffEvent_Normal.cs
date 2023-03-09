using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Framework
{
    /// <summary>
    /// 监听Buff事件数据基类，用以监听指定事件
    /// </summary>
    [BsonDeserializerRegister]
    public class ListenBuffEvent_Normal: ASkillSystemEvent<IBuffSystem>
    {
        /// <summary>
        /// Buff回调条件达成时会添加的Buff的节点Id
        /// </summary>
        [InfoBox("注意，是在节点编辑器中的Buff节点Id，而不是Buff自身的Id，别搞错了！")]
        [LabelText("Buff回调条件达成时会添加的Buff的节点Id")]
        public List<VTD_BuffInfo> BuffInfoWillBeAdded = new List<VTD_BuffInfo>();

        public override void Run(IBuffSystem a)
        {
            IBuffSystem aBuffSystemBase = a;
            //Log.Info($"直接添加_通过监听机制增加Buff");
            foreach (var buffInfo in this.BuffInfoWillBeAdded)
            {
                buffInfo.AutoAddBuff(aBuffSystemBase.BuffData.BelongToBuffDataSupportorId, buffInfo.BuffNodeId.Value, aBuffSystemBase.TheUnitFrom, aBuffSystemBase.TheUnitBelongto,
                    aBuffSystemBase.BelongtoRuntimeTree);
            }
        }
    }
}