namespace Framework
{
    public class ChangePropertyBuffSystem : ABuffSystemBase<ChangePropertyBuffData>
    {
        public override void OnExecute(uint currentFrame)
        {
            Unit target = this.GetBuffTarget();
        }

        public override void OnFinished(uint currentFrame)
        {
            switch (this.BuffData.BuffWorkType)
            {
                case BuffWorkTypes.ChangeAttackValue:
                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                        .RemoveDataModifier(NumericType.AttackAdd.ToString(), dataModifier, NumericType.AttackAdd);
                    break;
                case BuffWorkTypes.ChangeMagic:
                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                        .RemoveDataModifier(NumericType.Mp.ToString(), dataModifier, NumericType.Mp);
                    break;
                case BuffWorkTypes.ChangeSpeed:
                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                        .RemoveDataModifier(NumericType.Speed.ToString(), dataModifier, NumericType.Speed);
                    break;
            }

            if (this.dataModifier == null)
            {
                return;
            }

            ReferencePool.Release(dataModifier);
            dataModifier = null;
        }
    }
}