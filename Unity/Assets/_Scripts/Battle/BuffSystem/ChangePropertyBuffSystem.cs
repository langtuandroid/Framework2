namespace Framework
{
    public class ChangePropertyBuffSystem : ABuffSystemBase<ChangePropertyBuffData>
    {
        public override void OnExecute(long currentTime)
        {
            Unit target = this.GetBuffTarget();
            target.GetComponent<NumericComponent>()
                .ApplyChange(GetBuffDataWithTType.PropType, GetBuffDataWithTType.BasicValue);
        }

        public override void OnFinished(long currentTime)
        {
            this.GetBuffTarget().GetComponent<NumericComponent>()
                .ApplyChange(GetBuffDataWithTType.PropType, -GetBuffDataWithTType.BasicValue);
        }
    }
}