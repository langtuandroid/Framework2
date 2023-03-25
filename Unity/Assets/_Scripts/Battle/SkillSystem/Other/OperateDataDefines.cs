using Framework;

namespace Framework
{
    public abstract class OperateData: IReference
    {
        /// <summary>
        /// 操作发起者
        /// </summary>
        public Unit OperateCaster;

        /// <summary>
        /// 操作承受者
        /// </summary>
        public Unit OperateTaker;

        public virtual void Clear()
        {
            OperateCaster = null;
            OperateTaker = null;
        }
    }

    /// <summary>
    /// 伤害数据定义
    /// </summary>
    public class DamageData: OperateData
    {
        public SkillDamageTypes SkillDamageTypes;
        public int DamageValue;

        public DamageData InitData(SkillDamageTypes skillDamageTypes, int damageValue, Unit attackCaster, Unit attackReceiver)
        {
            SkillDamageTypes = skillDamageTypes;
            DamageValue = damageValue;
            this.OperateCaster = attackCaster;
            this.OperateTaker = attackReceiver;
            return this;
        }

        public override void Clear()
        {
            base.Clear();
            SkillDamageTypes = SkillDamageTypes.None;
            DamageValue = 0;
        }
    }

}