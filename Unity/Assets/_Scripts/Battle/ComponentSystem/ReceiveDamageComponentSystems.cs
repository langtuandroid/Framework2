    using Framework;

    public static class ReceiveDamageComponentSystems
    {
        /// <summary>
        /// 洗礼这个伤害值
        /// </summary>
        /// <param name="damageData">伤害数据</param>
        /// <returns></returns>
        public static float BaptismDamageData(this ReceiveDamageComponent self, DamageData damageData)
        {
            Unit damageTaker = self.GetParent<Unit>();
            damageData.OperateTaker = damageTaker;
            var numeric = self.GetParent<Entity>().GetComponent<NumericComponent>();
            switch (damageData.SkillDamageTypes)
            {
                case SkillDamageTypes.Physical:
                    damageData.DamageValue = (int)(damageData.DamageValue * (1 - numeric.GetAsFloat(NumericType.AckReduce)));
                    break;
                case SkillDamageTypes.Real:
                    break;
                case SkillDamageTypes.Magic:
                    damageData.DamageValue = (int)(damageData.DamageValue * (1 - numeric.GetAsFloat(NumericType.SpackReduce)));
                    break;
            }

            return damageData.DamageValue < 0 ? 0 : damageData.DamageValue;
        }

        /// <summary>
        /// 接受伤害
        /// </summary>
        /// <param name="damageData"></param>
        /// <returns></returns>
        public static void ReceiveDamage(this ReceiveDamageComponent self, DamageData damageData)
        {
            //如果已经死亡就不能继续受到伤害
            if (self.GetParent<Unit>().GetComponent<DeadComponent>() != null)
            {
                ReferencePool.Free(damageData);
                return;
            }

            damageData.OperateTaker = self.GetParent<Unit>();
            self.BaptismDamageData(damageData);

            float currentHp = self.GetParent<Unit>().GetComponent<NumericComponent>().GetByKey(NumericType.Hp);
            float finalHp = currentHp - damageData.DamageValue;

            if (finalHp <= 0)
            {
                finalHp = 0;
                self.GetParent<Unit>().GetComponent<NumericComponent>().Set(NumericType.Hp, finalHp);
            }
            else
            {
                self.GetParent<Unit>().GetComponent<NumericComponent>().Set(NumericType.Hp, finalHp);
            }
            
            ReferencePool.Free(damageData);
        }
    }