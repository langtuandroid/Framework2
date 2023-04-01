using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;

public partial class MapData : BaseConfig
{
    /// <summary> ID </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 模型地址 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string ModelPath { get; private set; }
/// <summary> 模型地址 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string ModelItemPath { get; private set; }
/// <summary> 结尾地图的路径 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string EndMapPath { get; private set; }
/// <summary> 模型外边环境的地址 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string EnvironmentPath { get; private set; }
/// <summary> 环境的长度 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public float EnvironmentLength { get; private set; }
/// <summary> 跳台的地址 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string JumpTablePath { get; private set; }
/// <summary>  </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int NodeId { get; private set; }

}

[Config("Assets/Res/Configs/MapData.bytes")]
public partial class MapDataFactory : ConfigSingleton<MapDataFactory>
{
    private Dictionary<int, MapData> dict = new Dictionary<int, MapData>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<MapData> list = new List<MapData>();

    public void Merge(MapDataFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (MapData config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
	
    partial void AfterEndInit();

    public MapData Get(int id)
    {
        this.dict.TryGetValue(id, out MapData MapData);

        if (MapData == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(MapData)}，配置id: {id}");
        }

        return MapData;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, MapData> GetAll()
    {
        return this.dict;
    }

    public MapData GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}