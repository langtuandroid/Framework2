using Framework;

public class ReceiveDamageComponent : Entity
{
    /// <summary>
    /// 洗礼这个伤害值
    /// </summary>
    /// <param name="damageData">伤害数据</param>
    /// <returns></returns>
    public void BaptismDamageData(DamageData damageData)
    {
        Unit damageTaker = GetParent<Unit>();
        damageData.OperateTaker = damageTaker;
        var numeric = GetParent<Entity>().GetComponent<NumericComponent>();
        switch (damageData.SkillDamageTypes)
        {
            case SkillDamageTypes.Physical:
                damageData.DamageValue -= damageData.DamageValue * numeric.GetAsInt(NumericType.AckReduce);
                break;
            case SkillDamageTypes.Real:
                break;
            case SkillDamageTypes.Magic:
                damageData.DamageValue -= damageData.DamageValue * numeric.GetAsInt(NumericType.SpackReduce);
                break;
        }
    }

    /// <summary>
    /// 接受伤害
    /// </summary>
    /// <param name="damageData"></param>
    /// <returns></returns>
    public void ReceiveDamage(DamageData damageData)
    {
        //如果已经死亡就不能继续受到伤害
        if (GetParent<Unit>().GetComponent<DeadComponent>() != null)
        {
            ReferencePool.Free(damageData);
            return;
        }

        int currentHp = GetParent<Unit>().GetComponent<NumericComponent>().GetByKey(NumericType.Hp);
        int finalHp = currentHp - damageData.DamageValue;

        if (finalHp <= 0)
        {
            finalHp = 0;
            GetParent<Unit>().GetComponent<NumericComponent>().Set(NumericType.Hp, finalHp);
        }
        else
        {
            GetParent<Unit>().GetComponent<NumericComponent>().Set(NumericType.Hp, finalHp);
        }

        ReferencePool.Free(damageData);
    }
}