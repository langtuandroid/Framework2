namespace Framework
{
    public class ChangePropertyBuffSystem : ABuffSystemBase<ChangePropertyBuffData>
    {
        public override void OnExecute(float currentTime)
        {
            Unit target = this.GetBuffTarget();
            target.GetComponent<NumericComponent>().ApplyChange(GetBuffDataWithTType.PropType, GetBuffDataWithTType.BasicValue.GetValue(BelongtoRuntimeTree.GetBlackboard()));
        }

        public override void OnFinished(float currentTime)
        {
            this.GetBuffTarget().GetComponent<NumericComponent>()
                .ApplyChange(GetBuffDataWithTType.PropType, -GetBuffDataWithTType.BasicValue.GetValue(BelongtoRuntimeTree.GetBlackboard()));
        }
    }
}