using System.Collections.Generic;
using Framework;
using Unity.Mathematics;

/// <summary>
/// 炮塔配置表。
/// </summary>
public class TowerData
{
    /// <summary>
    /// 获取配置编号。
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 获取炮塔名字Id。
    /// </summary>
    public string Name => $"炮塔{Id}";

    /// <summary>
    /// 获取图标名称。
    /// </summary>
    public string Icon { get; set; }

    public string PreviewPath { get; set; }

    /// <summary>
    /// 获取占用面积。
    /// </summary>
    public int2 Dimensions { get; set; }

    public TowerLevelData[] LevelDatas { get; set; }

    public static Dictionary<int, TowerData> Data;

    public static TowerData Get(int id)
    {
        if (Data.TryGetValue(id, out var data))
        {
            return data;
        }

        Log.Warning($"配置找不到，配置表名: {nameof(TowerData)}，配置id: {id}");
        return null;
    }
}