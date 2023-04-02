using Framework;

public class ReceiveDamageComponent : Unit
{
    /// <summary>
    /// 洗礼这个伤害值
    /// </summary>
    /// <param name="damageData">伤害数据</param>
    /// <returns></returns>
    public float BaptismDamageData(DamageData damageData)
    {
        Unit damageTaker = GetParent<Unit>();
        damageData.OperateTaker = damageTaker;
        var numeric = GetParent<Entity>().GetComponent<NumericComponent>();
        switch (damageData.SkillDamageTypes)
        {
            case SkillDamageTypes.Physical:
                damageData.DamageValue =
                    (int)(damageData.DamageValue * (1 - numeric.GetAsFloat(NumericType.AckReduce)));
                break;
            case SkillDamageTypes.Real:
                break;
            case SkillDamageTypes.Magic:
                damageData.DamageValue =
                    (int)(damageData.DamageValue * (1 - numeric.GetAsFloat(NumericType.SpackReduce)));
                break;
        }

        return damageData.DamageValue < 0 ? 0 : damageData.DamageValue;
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

        damageData.OperateTaker = GetParent<Unit>();
        BaptismDamageData(damageData);

        float currentHp = GetParent<Unit>().GetComponent<NumericComponent>().GetByKey(NumericType.Hp);
        float finalHp = currentHp - damageData.DamageValue;

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