using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;
using Newtonsoft.Json;

public partial class LanguageData : BaseConfig
{
    /// <summary> ID </summary>
	public int ID { get; private set; }
/// <summary> 描述 </summary>
	public string Desc { get; private set; }
/// <summary> 英语 </summary>
	public string English { get; private set; }
/// <summary> 葡语 </summary>
	public string BR { get; private set; }

}

[Config("Assets/Res/Configs/LanguageData.bytes")]
public class LanguageDataFactory : ConfigSingleton<LanguageDataFactory>
{
    private Dictionary<int, LanguageData> dict = new Dictionary<int, LanguageData>();

    [JsonProperty] 
    private List<LanguageData> list = new List<LanguageData>();

    public void Merge(object o)
    {
        LanguageDataFactory s = o as LanguageDataFactory;
        this.list.AddRange(s.list);
    }

    [OnDeserialized]
    public void ProtoEndInit(StreamingContext context)
    {
        foreach (LanguageData config in list)
        {
            config.AfterInit();
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }

    public LanguageData Get(int id)
    {
        this.dict.TryGetValue(id, out LanguageData LanguageData);

        if (LanguageData == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(LanguageData)}，配置id: {id}");
        }

        return LanguageData;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, LanguageData> GetAll()
    {
        return this.dict;
    }

    public LanguageData GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}