using Framework;

/// <summary>
/// 造成伤害组件
/// </summary>
public class CastDamageComponent : Entity, IAwakeSystem
{
    /// <summary>
    /// 洗礼这个伤害值
    /// </summary>
    /// <param name="damageData">伤害数据</param>
    /// <returns></returns>
    public float BaptismDamageData(DamageData damageData)
    {
        damageData.OperateCaster = this.GetParent<Unit>();
        var numeric = GetParent<Entity>().GetComponent<NumericComponent>();
        switch (damageData.SkillDamageTypes)
        {
            case SkillDamageTypes.Physical:
                damageData.DamageValue += (damageData.DamageValue * numeric.GetAsInt(NumericType.PhysicalMul));
                break;
            case SkillDamageTypes.Real:
                damageData.DamageValue += (damageData.DamageValue * numeric.GetAsInt(NumericType.RealAckMul));
                break;
            case SkillDamageTypes.Magic:
                damageData.DamageValue += (damageData.DamageValue * numeric.GetAsInt(NumericType.SpAckMul));
                break;
        }
        return damageData.DamageValue < 0 ? 0 : damageData.DamageValue;
    }

    public void Awake()
    {
    }
}