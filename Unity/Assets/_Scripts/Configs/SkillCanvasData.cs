using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;

public partial class SkillCanvasData : BaseConfig
{
    /// <summary> ID </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 对应技能图数据Id </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public long NPBehaveId { get; private set; }
/// <summary> 归属技能Id </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public long BelongToSkillId { get; private set; }
/// <summary> 资源名 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string SkillConfigPath { get; private set; }

}

[Config("Assets/Res/Configs/SkillCanvasData.json")]
public partial class SkillCanvasDataFactory : ConfigSingleton<SkillCanvasDataFactory>
{
    private Dictionary<int, SkillCanvasData> dict = new Dictionary<int, SkillCanvasData>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<SkillCanvasData> list = new List<SkillCanvasData>();

    public void Merge(SkillCanvasDataFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (SkillCanvasData config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
	
    partial void AfterEndInit();

    public SkillCanvasData Get(int id)
    {
        this.dict.TryGetValue(id, out SkillCanvasData SkillCanvasData);

        if (SkillCanvasData == null)
        {
            Log.Error($"配置找不到，配置表名: {nameof(SkillCanvasData)}，配置id: {id}");
        }

        return SkillCanvasData;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, SkillCanvasData> GetAll()
    {
        return this.dict;
    }

    public SkillCanvasData GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}