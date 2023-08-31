using System.Collections.Generic;
using Framework;

public partial class BehaveConfig : BaseConfig
{
    /// <summary> Id </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 对应AI图Id </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public long NPBehaveId { get; private set; }
/// <summary> 资源名 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string ConfigPath { get; private set; }

}

[Config("Assets/Res/Configs/BehaveConfig.json")]
public partial class BehaveConfigFactory : ConfigSingleton<BehaveConfigFactory>
{
    private Dictionary<int, BehaveConfig> dict = new Dictionary<int, BehaveConfig>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<BehaveConfig> list = new List<BehaveConfig>();

    public void Merge(BehaveConfigFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (BehaveConfig config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
    
    partial void AfterEndInit();

    public BehaveConfig Get(int id)
    {
        this.dict.TryGetValue(id, out BehaveConfig BehaveConfig);

        if (BehaveConfig == null)
        {
            Log.Warning($"配置找不到，配置表名: {nameof(BehaveConfig)}，配置id: {id}");
        }

        return BehaveConfig;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, BehaveConfig> GetAll()
    {
        return this.dict;
    }

    public BehaveConfig GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}