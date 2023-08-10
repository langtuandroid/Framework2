
namespace Framework
{
    [NumericWatcher(SceneType.Battle, NumericType.Spdef)]
    public class SpackReduceWatcher : INumericWatcher
    {
        public void Run(Unit unit, NumericChange args)
        {
            var numeric = unit.GetComponent<NumericComponent>();
            var spdef = numeric.GetAsFloat(args.NumericType);
            // 根据魔抗计算魔法伤害减免
            var spdefValue = spdef / 100;
            numeric.Set(NumericType.SpackReduce, spdefValue);
        }
    }

    [NumericWatcher(SceneType.Battle, NumericType.Armor)]
    public class AckReduceWatcher : INumericWatcher
    {
        public void Run(Unit unit, NumericChange args)
        {
            var numeric = unit.GetComponent<NumericComponent>();
            var spdef = numeric.GetAsFloat(args.NumericType);
            // 根据魔抗计算魔法伤害减免
            var spdefValue = spdef / 100;
            numeric.Set(NumericType.AckReduce, spdefValue);
        }
    }
}