namespace Framework
{
    public class ChangePropertyBuffSystem : ABuffSystemBase<ChangePropertyBuffData>
    {
        public override void OnExecute(uint currentFrame)
        {
            Unit target = this.GetBuffTarget();
            target.GetComponent<NumericComponent>().ApplyChange(GetBuffDataWithTType.PropType, GetBuffDataWithTType.BasicValue);
        }

        public override void OnFinished(uint currentFrame)
        {
            this.GetBuffTarget().GetComponent<NumericComponent>()
                .ApplyChange(GetBuffDataWithTType.PropType, -GetBuffDataWithTType.BasicValue);
        }
    }
}