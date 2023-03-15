namespace Framework
{
    /// <summary>
    /// 这里使用的瞬时治疗，如果要做持续治疗，参考持续伤害部分
    /// </summary>
    public class TreatmentBuffSystem : ABuffSystemBase<TreatmentBuffData>
    {
        public override void OnExecute(float currentTime)
        {
            float finalTreatValue;
            finalTreatValue = BuffDataCalculateHelper.CalculateCurrentData(this);
        }
    }
}