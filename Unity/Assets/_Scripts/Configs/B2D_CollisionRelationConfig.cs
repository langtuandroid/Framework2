using System.Collections.Generic;
using Framework;

public partial class B2D_CollisionRelationConfig : BaseConfig
{
    /// <summary> ID </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 对应碰撞数据配置Id </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ColliderConfigId { get; private set; }
/// <summary> 对应碰撞处理者名称 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string ColliderHandlerName { get; private set; }
/// <summary> 友方英雄碰撞 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public bool FriendlyHero { get; private set; }
/// <summary> 右方小兵碰撞 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public bool FriendlySoldier { get; private set; }
/// <summary> 敌方英雄碰撞 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public bool EnemyHero { get; private set; }
/// <summary> 敌方小兵碰撞 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public bool EnemySoldier { get; private set; }

}

[Config("Assets/Res/Configs/B2D_CollisionRelationConfig.json")]
public partial class B2D_CollisionRelationConfigFactory : ConfigSingleton<B2D_CollisionRelationConfigFactory>
{
    private Dictionary<int, B2D_CollisionRelationConfig> dict = new Dictionary<int, B2D_CollisionRelationConfig>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<B2D_CollisionRelationConfig> list = new List<B2D_CollisionRelationConfig>();

    public void Merge(B2D_CollisionRelationConfigFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (B2D_CollisionRelationConfig config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
    
    partial void AfterEndInit();

    public B2D_CollisionRelationConfig Get(int id)
    {
        this.dict.TryGetValue(id, out B2D_CollisionRelationConfig B2D_CollisionRelationConfig);

        if (B2D_CollisionRelationConfig == null)
        {
            Log.Warning($"配置找不到，配置表名: {nameof(B2D_CollisionRelationConfig)}，配置id: {id}");
        }

        return B2D_CollisionRelationConfig;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, B2D_CollisionRelationConfig> GetAll()
    {
        return this.dict;
    }

    public B2D_CollisionRelationConfig GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}