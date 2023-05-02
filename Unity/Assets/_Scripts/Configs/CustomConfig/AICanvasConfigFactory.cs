using System.Collections.Generic;
using Framework;

public partial class AICanvasConfigFactory
{
    private Dictionary<long, AICanvasConfig> npDataId2Data = new Dictionary<long, AICanvasConfig>();
    partial void AfterEndInit()
    {
        foreach (var data in dict.Values)
        {
            npDataId2Data[data.NPBehaveId] = data;
        }
    }

    public AICanvasConfig GetByNpDataId(long npDataId)
    {
        this.npDataId2Data.TryGetValue(npDataId, out AICanvasConfig aiCanvasConfig);

        if (aiCanvasConfig == null)
        {
            Log.Error($"配置找不到，配置表名: {nameof(aiCanvasConfig)}，配置id: {npDataId}");
        }

        return aiCanvasConfig;
    }
}