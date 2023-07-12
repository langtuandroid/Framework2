using System.Collections.Generic;
using Framework;

public partial class UnitConfig : BaseConfig
{
    /// <summary> ID </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 碰撞半径 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public float ColliderRadius { get; private set; }
/// <summary> 攻击距离 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public float AttackRange { get; private set; }

}

[Config("Assets/Res/Configs/UnitConfig.json")]
public partial class UnitConfigFactory : ConfigSingleton<UnitConfigFactory>
{
    private Dictionary<int, UnitConfig> dict = new Dictionary<int, UnitConfig>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<UnitConfig> list = new List<UnitConfig>();

    public void Merge(UnitConfigFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (UnitConfig config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
    
    partial void AfterEndInit();

    public UnitConfig Get(int id)
    {
        this.dict.TryGetValue(id, out UnitConfig UnitConfig);

        if (UnitConfig == null)
        {
            Log.Warning($"配置找不到，配置表名: {nameof(UnitConfig)}，配置id: {id}");
        }

        return UnitConfig;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, UnitConfig> GetAll()
    {
        return this.dict;
    }

    public UnitConfig GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}