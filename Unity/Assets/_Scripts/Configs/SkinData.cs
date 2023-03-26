using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;
using Newtonsoft.Json;

public partial class SkinData : BaseConfig
{
    /// <summary> ID </summary>
	[JsonProperty]
	public int ID { get; private set; }
/// <summary> 模型路径 </summary>
	[JsonProperty]
	public string ModelPath { get; private set; }
/// <summary> 图片路径 </summary>
	[JsonProperty]
	public string SpritePath { get; private set; }
/// <summary> 解锁需要的数量 </summary>
	[JsonProperty]
	public int Value { get; private set; }

}

[Config("Assets/Res/Configs/SkinData.bytes")]
public class SkinDataFactory : ConfigSingleton<SkinDataFactory>
{
    private Dictionary<int, SkinData> dict = new Dictionary<int, SkinData>();

    [JsonProperty] 
    private List<SkinData> list = new List<SkinData>();

    public void Merge(object o)
    {
        SkinDataFactory s = o as SkinDataFactory;
        this.list.AddRange(s.list);
    }

    [OnDeserialized]
    public void ProtoEndInit(StreamingContext context)
    {
        foreach (SkinData config in list)
        {
            config.AfterInit();
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }

    public SkinData Get(int id)
    {
        this.dict.TryGetValue(id, out SkinData SkinData);

        if (SkinData == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(SkinData)}，配置id: {id}");
        }

        return SkinData;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, SkinData> GetAll()
    {
        return this.dict;
    }

    public SkinData GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}