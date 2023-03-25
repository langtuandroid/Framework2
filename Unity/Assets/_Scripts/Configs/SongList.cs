using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;
using Newtonsoft.Json;

public partial class SongList : BaseConfig
{
    /// <summary> id </summary>
	public int ID { get; private set; }
/// <summary> 编号 </summary>
	public int Index { get; private set; }
/// <summary> 歌曲名称 </summary>
	public string SongName { get; private set; }
/// <summary> 歌手名称 </summary>
	public string Artist { get; private set; }
/// <summary> 难度 </summary>
	public string Difficulty { get; private set; }
/// <summary> 属性 </summary>
	public string Property { get; private set; }
/// <summary> 曲风 </summary>
	public string Style { get; private set; }
/// <summary> 锁状态（1-未锁定，2-金币锁定，3-广告锁定） </summary>
	public int Lock { get; private set; }
/// <summary> 解锁价格 </summary>
	public int Price { get; private set; }
/// <summary> 谱子路径 </summary>
	public string MapPath { get; private set; }
/// <summary> 歌曲路径 </summary>
	public string SongPath { get; private set; }
/// <summary> 歌曲封面 </summary>
	public string Cover { get; private set; }

}

[Config("Assets/Res/Configs/SongList.bytes")]
public class SongListFactory : ConfigSingleton<SongListFactory>
{
    private Dictionary<int, SongList> dict = new Dictionary<int, SongList>();

    [JsonProperty] 
    private List<SongList> list = new List<SongList>();

    public void Merge(object o)
    {
        SongListFactory s = o as SongListFactory;
        this.list.AddRange(s.list);
    }

    [OnDeserialized]
    public void ProtoEndInit(StreamingContext context)
    {
        foreach (SongList config in list)
        {
            config.AfterInit();
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }

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