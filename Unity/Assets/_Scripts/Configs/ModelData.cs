using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;

public partial class ModelData : BaseConfig
{
    /// <summary> ID </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 模型路径 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string ModelPath { get; private set; }
/// <summary> aa </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
	public Dictionary<int, string> TestDic { get; private set; }

}

[Config("Assets/Res/Configs/ModelData.bytes")]
public partial class ModelDataFactory : ConfigSingleton<ModelDataFactory>
{
    private Dictionary<int, ModelData> dict = new Dictionary<int, ModelData>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<ModelData> list = new List<ModelData>();

    public void Merge(ModelDataFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (ModelData config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
	
    partial void AfterEndInit();

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