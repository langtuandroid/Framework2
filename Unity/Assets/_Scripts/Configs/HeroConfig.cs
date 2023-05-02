using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;

public partial class HeroConfig : BaseConfig
{
    /// <summary> ID </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 碰撞半径 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public float ColliderRadius { get; private set; }

}

[Config("Assets/Res/Configs/HeroConfig.json")]
public partial class HeroConfigFactory : ConfigSingleton<HeroConfigFactory>
{
    private Dictionary<int, HeroConfig> dict = new Dictionary<int, HeroConfig>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<HeroConfig> list = new List<HeroConfig>();

    public void Merge(HeroConfigFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (HeroConfig config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
	
    partial void AfterEndInit();

    public HeroConfig Get(int id)
    {
        this.dict.TryGetValue(id, out HeroConfig HeroConfig);

        if (HeroConfig == null)
        {
            Log.Error($"配置找不到，配置表名: {nameof(HeroConfig)}，配置id: {id}");
        }

        return HeroConfig;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, HeroConfig> GetAll()
    {
        return this.dict;
    }

    public HeroConfig GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}