using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;

public partial class SoldierConfig : BaseConfig
{
    /// <summary> ID </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 碰撞半径 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public float ColliderRadius { get; private set; }

}

[Config("Assets/Res/Configs/SoldierConfig.json")]
public partial class SoldierConfigFactory : ConfigSingleton<SoldierConfigFactory>
{
    private Dictionary<int, SoldierConfig> dict = new Dictionary<int, SoldierConfig>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<SoldierConfig> list = new List<SoldierConfig>();

    public void Merge(SoldierConfigFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (SoldierConfig config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
	
    partial void AfterEndInit();

    public SoldierConfig Get(int id)
    {
        this.dict.TryGetValue(id, out SoldierConfig SoldierConfig);

        if (SoldierConfig == null)
        {
            Log.Error($"配置找不到，配置表名: {nameof(SoldierConfig)}，配置id: {id}");
        }

        return SoldierConfig;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, SoldierConfig> GetAll()
    {
        return this.dict;
    }

    public SoldierConfig GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}