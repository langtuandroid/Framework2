/// <summary>
/// 炮塔等级配置表。
/// </summary>
public class TowerLevelData
{
    /// <summary>
    /// 获取配置编号。
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 获取炮塔描述Id。
    /// </summary>
    public string Des => $"lv{Id}炮塔的描述";

    /// <summary>
    /// 获取炮塔描述Id。
    /// </summary>
    public string UpgradeDes => $"lv{Id}炮塔的升级描述";

    public string ModelPath { get; set; }

    /// <summary>
    /// 获取建造能量。
    /// </summary>
    public int BuildEnergy { get; set; }

    /// <summary>
    /// 获取出售能量。
    /// </summary>
    public int SellEnergy { get; set; }

    public float Range { get; set; }
}