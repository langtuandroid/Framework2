using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;
using Newtonsoft.Json;

public partial class SkillCanvasData : BaseConfig
{
    /// <summary> ID </summary>
	[JsonProperty]
	public int ID { get; private set; }
/// <summary> 对应技能图数据Id </summary>
	[JsonProperty]
	public long NPBehaveId { get; private set; }
/// <summary> 归属技能Id </summary>
	[JsonProperty]
	public long BelongToSkillId { get; private set; }
/// <summary> 资源名 </summary>
	[JsonProperty]
	public string SkillConfigName { get; private set; }

}

[Config("Assets/Res/Configs/SkillCanvasData.bytes")]
public class SkillCanvasDataFactory : ConfigSingleton<SkillCanvasDataFactory>
{
    private Dictionary<int, SkillCanvasData> dict = new Dictionary<int, SkillCanvasData>();

    [JsonProperty] 
    private List<SkillCanvasData> list = new List<SkillCanvasData>();

    public void Merge(object o)
    {
        SkillCanvasDataFactory s = o as SkillCanvasDataFactory;
        this.list.AddRange(s.list);
    }

    [OnDeserialized]
    public void ProtoEndInit(StreamingContext context)
    {
        foreach (SkillCanvasData config in list)
        {
            config.AfterInit();
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }

    public SkillCanvasData Get(int id)
    {
        this.dict.TryGetValue(id, out SkillCanvasData SkillCanvasData);

        if (SkillCanvasData == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(SkillCanvasData)}，配置id: {id}");
        }

        return SkillCanvasData;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, SkillCanvasData> GetAll()
    {
        return this.dict;
    }

    public SkillCanvasData GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}