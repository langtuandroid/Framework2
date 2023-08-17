namespace Framework
{
    /// <summary>
    /// 持续伤害，一般描述为X秒内造成Y伤害，或者每X秒造成Y伤害
    /// </summary>
    public class SustainDamageBuffSystem : ABuffSystemBase<SustainDamageBuffData>
    {
        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private long selfNextExcuteTime = 0;

        public override void OnExecute(long currentTime)
        {
            ExcuteDamage(currentTime);
            //Log.Info($"作用间隔为{selfNextimer - TimeHelper.Now()},持续时间为{temp.SustainTime},持续到{this.selfNextimer}");
        }

        public override void OnUpdate(long currentTime)
        {
            if (currentTime >= this.selfNextExcuteTime)
            {
                ExcuteDamage(currentTime);
            }
        }

        private void ExcuteDamage(long currentTime)
        {
            //强制类型转换为伤害Buff数据 
            SustainDamageBuffData temp = this.GetBuffDataWithTType;

            DamageData damageData = ReferencePool.Allocate<DamageData>().InitData(temp.DamageType,
                BuffDataCalculateHelper.CalculateCurrentData(this), this.TheUnitFrom, this.TheUnitBelongto);

            this.TheUnitFrom.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);

            this.TheUnitBelongto.GetComponent<ReceiveDamageComponent>().BaptismDamageData(damageData);

            this.TheUnitBelongto.GetComponent<ReceiveDamageComponent>().ReceiveDamage(damageData);
            // //抛出伤害事件
            // this.GetBuffTarget().DomainScene().GetComponent<BattleEventSystemComponent>()
            //     .Run($"ExcuteDamage{this.TheUnitFrom.Id}", damageData);
            // //抛出受伤事件
            // this.GetBuffTarget().DomainScene().GetComponent<BattleEventSystemComponent>()
            //     .Run($"TakeDamage{this.GetBuffTarget().Id}", damageData);

            //设置下一个时间点
            this.selfNextExcuteTime = currentTime + (temp.WorkInternal);
        }
    }
}