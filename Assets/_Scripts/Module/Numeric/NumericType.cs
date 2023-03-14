using System.Reflection;

namespace Framework
{
   public static class NumericType
    {
        //最小值，小于此值的都被认为是原始属性
       public const int Max = 10000;
       public const int None = 1000;

       //生命值
       public const int Hp = 1001;

       //最大生命值
       public const int MaxHp = 1002;
       public const int MaxHpBase = MaxHp * 10 + 1;
       public const int MaxHpAdd = MaxHp * 10 + 2;
       public const int MaxHpPct = MaxHp * 10 + 3;
       public const int MaxHpFinalAdd = MaxHp * 10 + 4;
       public const int MaxHpFinalPct = MaxHp * 10 + 5;

       //速度
       public const int Speed = 1005;
       public const int SpeedBase = Speed * 10 + 1;
       public const int SpeedAdd = Speed * 10 + 2;
       public const int SpeedPct = Speed * 10 + 3;
       public const int SpeedFinalAdd = Speed * 10 + 4;
       public const int SpeedFinalPct = Speed * 10 + 5;

       //攻击力
       public const int Attack = 1006;
       public const int AttackBase = Attack * 10 + 1;
       public const int AttackAdd = Attack * 10 + 2;
       public const int AttackPct = Attack * 10 + 3;
       public const int AttackFinalAdd = Attack * 10 + 4;
       public const int AttackFinalPct = Attack * 10 + 5;

       //护甲
       public const int Armor = 1008;
       public const int ArmorBase = Armor * 10 + 1;
       public const int ArmorAdd = Armor * 10 + 2;
       public const int ArmorPct = Armor * 10 + 3;
       public const int ArmorFinalAdd = Armor * 10 + 4;
       public const int ArmorFinalPct = Armor * 10 + 5;

       //物理伤害减少 15% 根据护甲计算来的
       public const int AckReduce = 1009;
       
       //魔抗
       public const int Spdef = 1010;
       public const int SpdefBase = Spdef * 10 + 1;
       public const int SpdefAdd = Spdef * 10 + 2;
       public const int SpdefPct = Spdef * 10 + 3;
       public const int SpdefFinalAdd = Spdef * 10 + 4;
       public const int SpdefFinalPct = Spdef * 10 + 5;

       //魔法伤害减少 15% 根据魔抗计算来的
       public const int SpackReduce = 1011;

       public static DoubleMap<string, int> Str2TypeDoubleMap { get; }

       static NumericType()
       {
           Str2TypeDoubleMap = new DoubleMap<string, int>();
           var fields = typeof(NumericType).GetFields(BindingFlags.Public | BindingFlags.Static);
           foreach (var fieldInfo in fields)
           {
               var type = (int)fieldInfo.GetValue(null);
               if (type > Max)
               {
                   Str2TypeDoubleMap.Add(fieldInfo.Name, type);
               }
           }

           Str2TypeDoubleMap.Add(nameof(None), None);
           Str2TypeDoubleMap.Add(nameof(Hp), Hp);
       }
    }
}