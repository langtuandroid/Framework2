using System;
using Framework;

public class CastDamageComponent : Entity
{
    /// <summary>
    /// 洗礼这个伤害值
    /// </summary>
    /// <param name="damageData">伤害数据</param>
    /// <returns></returns>
    public float BaptismDamageData(DamageData damageData)
    {
        damageData.OperateCaster = this.GetParent<Unit>();
        switch (damageData.SkillDamageTypes)
        {
            case SkillDamageTypes.Physical:
                break;
            case SkillDamageTypes.Real:
                break;
            case SkillDamageTypes.Magic:
                break;
        }
        return damageData.DamageValue < 0 ? 0 : damageData.DamageValue;
    }
}