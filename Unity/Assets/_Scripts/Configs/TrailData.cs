using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;
using Newtonsoft.Json;

public partial class TrailData : BaseConfig
{
    /// <summary> ID </summary>
	public int ID { get; private set; }
/// <summary> 模型路径 </summary>
	public string TrailModelPath { get; private set; }
/// <summary> 图片路径 </summary>
	public string SpritePath { get; private set; }
/// <summary> 临时图片路径 </summary>
	public string tmpTrailModelPath { get; private set; }
/// <summary> 解锁需要的数量 </summary>
	public int Value { get; private set; }

}

[Config("Assets/Res/Configs/TrailData.bytes")]
public class TrailDataFactory : ConfigSingleton<TrailDataFactory>
{
    private Dictionary<int, TrailData> dict = new Dictionary<int, TrailData>();

    [JsonProperty] 
    private List<TrailData> list = new List<TrailData>();

    public void Merge(object o)
    {
        TrailDataFactory s = o as TrailDataFactory;
        this.list.AddRange(s.list);
    }

    [OnDeserialized]
    public void ProtoEndInit(StreamingContext context)
    {
        foreach (TrailData config in list)
        {
            config.AfterInit();
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }

    public TrailData Get(int id)
    {
        this.dict.TryGetValue(id, out TrailData TrailData);

        if (TrailData == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(TrailData)}，配置id: {id}");
        }

        return TrailData;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, TrailData> GetAll()
    {
        return this.dict;
    }

    public TrailData GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}