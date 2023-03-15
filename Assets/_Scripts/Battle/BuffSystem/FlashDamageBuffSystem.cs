namespace Framework
{
    /// <summary>
    /// 瞬时伤害
    /// </summary>
    public class FlashDamageBuffSystem : ABuffSystemBase<FlashDamageBuffData>
    {
        public override void OnExecute(float currentTime)
        {
            FlashDamageBuffData flashDamageBuffData = this.GetBuffDataWithTType;

            DamageData damageData = ReferencePool.Allocate<DamageData>().InitData(flashDamageBuffData.DamageType,
                BuffDataCalculateHelper.CalculateCurrentData(this), this.TheUnitFrom, this.TheUnitBelongto);

            damageData.DamageValue *= flashDamageBuffData.DamageFix;

            this.TheUnitFrom.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);

            float finalDamage =
                this.GetBuffTarget().GetComponent<ReceiveDamageComponent>().BaptismDamageData(damageData);
            
            if (finalDamage >= 0)
            {
                this.TheUnitBelongto.GetComponent<NumericComponent>().ApplyChange(NumericType.Hp, -finalDamage);
                //抛出伤害事件
                this.GetBuffTarget().DomainScene().GetComponent<BattleEventSystemComponent>().Run($"ExcuteDamage{this.TheUnitFrom.Id}", damageData);
                //抛出受伤事件
                this.GetBuffTarget().DomainScene().GetComponent<BattleEventSystemComponent>().Run($"TakeDamage{this.GetBuffTarget().Id}", damageData);
            }

            //TODO 从当前战斗Entity获取BattleEventSystem来Run事件
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    this.GetBuffTarget().DomainScene().GetComponent<BattleEventSystemComponent>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
        }
    }
}