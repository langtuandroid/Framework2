using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;

public partial class SongList : BaseConfig
{
    /// <summary> id </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int ID { get; private set; }
/// <summary> 编号 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int Index { get; private set; }
/// <summary> 歌曲名称 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string SongName { get; private set; }
/// <summary> 歌手名称 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string Artist { get; private set; }
/// <summary> 难度 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string Difficulty { get; private set; }
/// <summary> 属性 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string Property { get; private set; }
/// <summary> 曲风 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string Style { get; private set; }
/// <summary> 锁状态（1-未锁定，2-金币锁定，3-广告锁定） </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int Lock { get; private set; }
/// <summary> 解锁价格 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public int Price { get; private set; }
/// <summary> 谱子路径 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string MapPath { get; private set; }
/// <summary> 歌曲路径 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string SongPath { get; private set; }
/// <summary> 歌曲封面 </summary>
	[MongoDB.Bson.Serialization.Attributes.BsonElement]
	public string Cover { get; private set; }

}

[Config("Assets/Res/Configs/SongList.json")]
public partial class SongListFactory : ConfigSingleton<SongListFactory>
{
    private Dictionary<int, SongList> dict = new Dictionary<int, SongList>();

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    private List<SongList> list = new List<SongList>();

    public void Merge(SongListFactory o)
    {
        this.list.AddRange(o.list);
    }

    public override void EndInit()
    {
        foreach (SongList config in list)
        {
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }
	
    partial void AfterEndInit();

    public SongList Get(int id)
    {
        this.dict.TryGetValue(id, out SongList SongList);

        if (SongList == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(SongList)}，配置id: {id}");
        }

        return SongList;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, SongList> GetAll()
    {
        return this.dict;
    }

    public SongList GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}