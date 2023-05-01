using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;

public partial class B2DCollisionRelationConfig : BaseConfig
{
    /// <summary> ID </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 对应碰撞数据配置Id </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int B2S_ColliderConfigId { get; private set; }
/// <summary> 对应碰撞处理者名称 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string B2S_ColliderHandlerName { get; private set; }
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

[Config("Assets/Res/Configs/B2DCollisionRelationConfig.json")]
public partial class B2DCollisionRelationConfigFactory : ConfigSingleton<B2DCollisionRelationConfigFactory>
{
    private Dictionary<int, B2DCollisionRelationConfig> dict = new Dictionary<int, B2DCollisionRelationConfig>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<B2DCollisionRelationConfig> list = new List<B2DCollisionRelationConfig>();

    public void Merge(B2DCollisionRelationConfigFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (B2DCollisionRelationConfig config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
	
    partial void AfterEndInit();

    public B2DCollisionRelationConfig Get(int id)
    {
        this.dict.TryGetValue(id, out B2DCollisionRelationConfig B2DCollisionRelationConfig);

        if (B2DCollisionRelationConfig == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(B2DCollisionRelationConfig)}，配置id: {id}");
        }

        return B2DCollisionRelationConfig;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, B2DCollisionRelationConfig> GetAll()
    {
        return this.dict;
    }

    public B2DCollisionRelationConfig GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}