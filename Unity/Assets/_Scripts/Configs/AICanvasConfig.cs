using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;

public partial class AICanvasConfig : BaseConfig
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

[Config("Assets/Res/Configs/AICanvasConfig.json")]
public partial class AICanvasConfigFactory : ConfigSingleton<AICanvasConfigFactory>
{
    private Dictionary<int, AICanvasConfig> dict = new Dictionary<int, AICanvasConfig>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<AICanvasConfig> list = new List<AICanvasConfig>();

    public void Merge(AICanvasConfigFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (AICanvasConfig config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
	
    partial void AfterEndInit();

    public AICanvasConfig Get(int id)
    {
        this.dict.TryGetValue(id, out AICanvasConfig AICanvasConfig);

        if (AICanvasConfig == null)
        {
            Log.Error($"配置找不到，配置表名: {nameof(AICanvasConfig)}，配置id: {id}");
        }

        return AICanvasConfig;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, AICanvasConfig> GetAll()
    {
        return this.dict;
    }

    public AICanvasConfig GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}