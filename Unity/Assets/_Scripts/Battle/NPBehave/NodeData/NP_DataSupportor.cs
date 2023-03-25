using System.Collections.Generic;
using Sirenix.OdinInspector;

/// <summary>
/// 技能配置数据载体
/// </summary>
[HideLabel]
public class NP_DataSupportor
{
    // [BoxGroup("技能中的Buff数据结点")]
    public Dictionary<long, BuffNodeDataBase> BuffNodeDataDic = new Dictionary<long, BuffNodeDataBase>();

    public NP_DataSupportorBase NpDataSupportorBase = new NP_DataSupportorBase();
}