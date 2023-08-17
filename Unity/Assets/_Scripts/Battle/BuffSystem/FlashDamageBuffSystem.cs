namespace Framework
{
    /// <summary>
    /// 瞬时伤害
    /// </summary>
    public class FlashDamageBuffSystem : ABuffSystemBase<FlashDamageBuffData>
    {
        public override void OnExecute(long currentTime)
        {
            FlashDamageBuffData flashDamageBuffData = this.GetBuffDataWithTType;

            DamageData damageData = ReferencePool.Allocate<DamageData>().InitData(flashDamageBuffData.DamageType,
                BuffDataCalculateHelper.CalculateCurrentData(this), this.TheUnitFrom, this.TheUnitBelongto);

            this.TheUnitFrom.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);

            this.GetBuffTarget().GetComponent<ReceiveDamageComponent>().BaptismDamageData(damageData);

            this.GetBuffTarget().GetComponent<ReceiveDamageComponent>().ReceiveDamage(damageData);
            //抛出伤害事件
            this.GetBuffTarget().DomainScene().GetComponent<BattleEventSystemComponent>()
                .Run(BattleEvent.DoDamage, damageData);

            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    this.GetBuffTarget().DomainScene().GetComponent<BattleEventSystemComponent>()
                        .Run(eventId.Value, this);
                }
            }
        }
    }
}