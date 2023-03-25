using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;
using Newtonsoft.Json;

public partial class ModelData : BaseConfig
{
    /// <summary> ID </summary>
	public int ID { get; private set; }
/// <summary> 模型路径 </summary>
	public string ModelPath { get; private set; }

}

[Config("Assets/Res/Configs/ModelData.bytes")]
public class ModelDataFactory : ConfigSingleton<ModelDataFactory>
{
    private Dictionary<int, ModelData> dict = new Dictionary<int, ModelData>();

    [JsonProperty] 
    private List<ModelData> list = new List<ModelData>();

    public void Merge(object o)
    {
        ModelDataFactory s = o as ModelDataFactory;
        this.list.AddRange(s.list);
    }

    [OnDeserialized]
    public void ProtoEndInit(StreamingContext context)
    {
        foreach (ModelData config in list)
        {
            config.AfterInit();
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }

    public ModelData Get(int id)
    {
        this.dict.TryGetValue(id, out ModelData ModelData);

        if (ModelData == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(ModelData)}，配置id: {id}");
        }

        return ModelData;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, ModelData> GetAll()
    {
        return this.dict;
    }

    public ModelData GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}