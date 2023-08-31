using System.Collections.Generic;
using Framework;

public partial class SkillBehaveConfig : BaseConfig
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

[Config("Assets/Res/Configs/SkillBehaveConfig.json")]
public partial class SkillBehaveConfigFactory : ConfigSingleton<SkillBehaveConfigFactory>
{
    private Dictionary<int, SkillBehaveConfig> dict = new Dictionary<int, SkillBehaveConfig>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<SkillBehaveConfig> list = new List<SkillBehaveConfig>();

    public void Merge(SkillBehaveConfigFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (SkillBehaveConfig config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
    
    partial void AfterEndInit();

    public SkillBehaveConfig Get(int id)
    {
        this.dict.TryGetValue(id, out SkillBehaveConfig SkillBehaveConfig);

        if (SkillBehaveConfig == null)
        {
            Log.Warning($"配置找不到，配置表名: {nameof(SkillBehaveConfig)}，配置id: {id}");
        }

        return SkillBehaveConfig;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, SkillBehaveConfig> GetAll()
    {
        return this.dict;
    }

    public SkillBehaveConfig GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}