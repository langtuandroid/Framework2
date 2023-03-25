namespace Framework
{
    public class RefreshTargetBuffTimeBuffSystem: ABuffSystemBase<RefreshTargetBuffTimeBuffData>
    {
        public override void OnExecute(float currentTime)
        {
            BuffManagerComponent buffManagerComponent = this.GetBuffTarget().GetComponent<BuffManagerComponent>();

            foreach (var buffNodeId in this.GetBuffDataWithTType.TheBuffNodeIdToBeRefreshed)
            {
                //Log.Info($"准备刷新指定Buff——{buffId}持续时间");
                IBuffSystem aBuffSystemBase =
                    buffManagerComponent.GetBuffById(
                        (this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[buffNodeId.Value] as NormalBuffNodeData).BuffData
                        .BuffId);
                if (aBuffSystemBase != null && aBuffSystemBase.BuffData.SustainTime + 1 > 0)
                {
                    // Log.Info(
                    //     $"刷新了指定Buff——{buffId}持续时间{TimeHelper.Now() + buffSystemBase.MSkillBuffDataBase.SustainTime},原本为{buffSystemBase.MaxLimitTime}");
                    aBuffSystemBase.MaxLimitTime = currentTime + aBuffSystemBase.BuffData.SustainTime;
                }
            }
        }
    }
}