namespace Framework
{
   public static class NumericType
    {
        //最小值，小于此值的都被认为是原始属性
       public const int Max = 10000;

       //生命值
       public const int Hp = 1001;

       //最大生命值
       public const int MaxHp = 1002;
       public const int MaxHpBase = MaxHp * 10 + 1;
       public const int MaxHpAdd = MaxHp * 10 + 2;

       //魔法值
       public const int Mp = 1003;

       //最大魔法值
       public const int MaxMp = 1004;
       public const int MaxMpBase = MaxMp * 10 + 1;
       public const int MaxMpAdd = MaxMp * 10 + 2;

       //速度
       public const int Speed = 1005;
       public const int SpeedBase = Speed * 10 + 1;
       public const int SpeedAdd = Speed * 10 + 2;

       //攻击力
       public const int Attack = 1006;
       public const int AttackBase = Attack * 10 + 1;
       public const int AttackAdd = Attack * 10 + 2;

       //法强
       public const int MagicStrength = 1007;
       public const int MagicStrengthBase = MagicStrength * 10 + 1;
       public const int MagicStrengthAdd = MagicStrength * 10 + 2;

       //护甲
       public const int Armor = 1008;
       public const int ArmorBase = Armor * 10 + 1;
       public const int ArmorAdd = Armor * 10 + 2;

       //魔抗
       public const int MagicResistance = 1009;
       public const int MagicResistanceBase = MagicResistance * 10 + 1;
       public const int MagicResistanceAdd = MagicResistance * 10 + 2;

       //护甲穿透
       public const int ArmorPenetration = 1010;
       public const int ArmorPenetrationBase = ArmorPenetration * 10 + 1;
       public const int ArmorPenetrationAdd = ArmorPenetration * 10 + 2;

       //法术穿透
       public const int MagicPenetration = 1011;
       public const int MagicPenetrationBase = MagicPenetration * 10 + 1;
       public const int MagicPenetrationAdd = MagicPenetration * 10 + 2;

       //暴击率
       public const int CriticalStrikeProbability = 1012;

       //技能冷却缩减
       public const int SkillCD = 1013;

       //生命恢复
       public const int HPRec = 1014;
       public const int HPRecBase = HPRec * 10 + 1;
       public const int HPRecAdd = HPRec * 10 + 2;

       //魔法恢复
       public const int MPRec = 1015;
       public const int MPRecBase = MPRec * 10 + 1;
       public const int MPRecAdd = MPRec * 10 + 2;

       //攻击速度
       public const int AttackSpeed = 1016;
       public const int AttackSpeedBase = AttackSpeed * 10 + 1;
       public const int AttackSpeedAdd = AttackSpeed * 10 + 2;

       //攻速收益
       public const int AttackSpeedIncome = 1017;

       //等级
       public const int Level = 1018;

       //最大等级
       public const int MaxLevel = 1019;

       //暴击伤害
       public const int CriticalStrikeHarm = 1020;

       //攻击距离
       public const int AttackRange = 1021;
       public const int AttackRangeBase = AttackRange * 10 + 1;
       public const int AttackRangeAdd = AttackRange * 10 + 2;
    }
}