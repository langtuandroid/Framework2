using System;
using System.Collections.Generic;

public partial class SkillCanvasDataFactory
{
    private Dictionary<long, SkillCanvasData> npDataId2Data = new Dictionary<long, SkillCanvasData>();
    partial void AfterEndInit()
    {
        foreach (var data in dict.Values)
        {
            npDataId2Data[data.NPBehaveId] = data;
        }
    }

    public SkillCanvasData GetByNpDataId(long npDataId)
    {
        this.npDataId2Data.TryGetValue(npDataId, out SkillCanvasData SkillCanvasData);

        if (SkillCanvasData == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(SkillCanvasData)}，配置id: {npDataId}");
        }

        return SkillCanvasData;
    }
}