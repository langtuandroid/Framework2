using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;

public partial class LanguageData : BaseConfig
{
    /// <summary> ID </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 描述 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string Desc { get; private set; }
/// <summary> 英语 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string English { get; private set; }
/// <summary> 葡语 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string BR { get; private set; }

}

[Config("Assets/Res/Configs/LanguageData.json")]
public partial class LanguageDataFactory : ConfigSingleton<LanguageDataFactory>
{
    private Dictionary<int, LanguageData> dict = new Dictionary<int, LanguageData>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<LanguageData> list = new List<LanguageData>();

    public void Merge(LanguageDataFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (LanguageData config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
	
    partial void AfterEndInit();

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