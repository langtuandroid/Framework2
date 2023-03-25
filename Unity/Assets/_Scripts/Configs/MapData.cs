using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;
using Newtonsoft.Json;

public partial class MapData : BaseConfig
{
    /// <summary> ID </summary>
	public int ID { get; private set; }
/// <summary> 模型地址 </summary>
	public string ModelPath { get; private set; }
/// <summary> 模型地址 </summary>
	public string ModelItemPath { get; private set; }
/// <summary> 结尾地图的路径 </summary>
	public string EndMapPath { get; private set; }
/// <summary> 模型外边环境的地址 </summary>
	public string EnvironmentPath { get; private set; }
/// <summary> 环境的长度 </summary>
	public float EnvironmentLength { get; private set; }
/// <summary> 跳台的地址 </summary>
	public string JumpTablePath { get; private set; }
/// <summary>  </summary>
	public int NodeId { get; private set; }

}

[Config("Assets/Res/Configs/MapData.bytes")]
public class MapDataFactory : ConfigSingleton<MapDataFactory>
{
    private Dictionary<int, MapData> dict = new Dictionary<int, MapData>();

    [JsonProperty] 
    private List<MapData> list = new List<MapData>();

    public void Merge(object o)
    {
        MapDataFactory s = o as MapDataFactory;
        this.list.AddRange(s.list);
    }

    [OnDeserialized]
    public void ProtoEndInit(StreamingContext context)
    {
        foreach (MapData config in list)
        {
            config.AfterInit();
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }

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