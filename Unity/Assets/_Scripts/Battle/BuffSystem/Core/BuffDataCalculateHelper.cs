

namespace Framework
{
    /// <summary>
    /// 用以计算Buff最终数据的辅助类
    /// </summary>
    public static class BuffDataCalculateHelper
    {
        public static int CalculateCurrentData(IBuffSystem buffSystem)
        {
            var numericComponent = buffSystem.TheUnitFrom.GetComponent<NumericComponent>();
        
            BuffDataBase buffData = buffSystem.BuffData;
            int tempData = buffData.BasicValue;
        
            //依据基础数值的加成方式来获取对应数据
            switch (buffData.BaseBuffBaseDataEffectTypes)
            {
                case BuffBaseDataEffectTypes.FromHeroLevel:
                    break;
                case BuffBaseDataEffectTypes.FromSkillLevel:
                    break;
                case BuffBaseDataEffectTypes.FromHasLostLifeValue:
                    break;
                case BuffBaseDataEffectTypes.FromCurrentOverlay:
                    break;
            }
        
            //依据加成方式对伤害进行加成
            foreach (var additionValue in buffData.AdditionValue)
            {
                switch (additionValue.Key)
                {
                    case BuffAdditionTypes.Percentage_Physical:
                        tempData += (int)(additionValue.Value * numericComponent.GetByKey(NumericType.Attack));
                        break;
                    case BuffAdditionTypes.SelfOverlay_Mul:
                        tempData *= (int)(additionValue.Value * buffSystem.CurrentOverlay);
                        break;
                }
            }
        
            return tempData;
        }
    }
}