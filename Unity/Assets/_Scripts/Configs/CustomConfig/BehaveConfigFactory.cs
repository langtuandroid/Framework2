using System.Collections.Generic;
using Framework;

public partial class BehaveConfigFactory
{
    private Dictionary<long, BehaveConfig> npDataId2Data = new Dictionary<long, BehaveConfig>();
    partial void AfterEndInit()
    {
        foreach (var data in dict.Values)
        {
            npDataId2Data[data.NPBehaveId] = data;
        }
    }

    public BehaveConfig GetByNpRootNodeId(long rootNodeId)
    {
        this.npDataId2Data.TryGetValue(rootNodeId, out BehaveConfig BehaveConfig);

        if (BehaveConfig == null)
        {
            Log.Error($"配置找不到，配置表名: {nameof(BehaveConfig)}，配置id: {rootNodeId}");
        }

        return BehaveConfig;
    }
}