using System.Collections.Generic;
using Framework;
using NPBehave;
using UnityEngine;

public class NP_TreeDataRepositoryComponent : Entity
{
    /// <summary>
    /// 运行时的行为树仓库，注意，一定不能对这些数据做修改
    /// </summary>
    public Dictionary<long, NP_DataSupportor> NpRuntimeSkillTreesDatas = new Dictionary<long, NP_DataSupportor>();

    public NP_DataSupportor GetNPTreeData(long rootNodeId)
    {
        if (NpRuntimeSkillTreesDatas.TryGetValue(rootNodeId, out var dataSupportor)) return dataSupportor;
        var skillCanvasConfig = BehaveConfigFactory.Instance.GetByNpRootNodeId(rootNodeId);
        TextAsset textAsset =
            ResComponent.Instance.LoadAsset<TextAsset>(skillCanvasConfig.ConfigPath);

        if (textAsset.bytes.Length == 0) Log.Msg("没有读取到文件");

        dataSupportor = SerializeHelper.Deserialize<NP_DataSupportor>(textAsset.text);
        NpRuntimeSkillTreesDatas[rootNodeId] = dataSupportor;
        return dataSupportor;
    }

    /// <summary>
    /// 获取一棵树的所有数据（仅深拷贝黑板数据内容，而忽略例如BuffNodeDataDic的数据内容）
    /// </summary>
    /// <param name="rootNodeId"></param>
    /// <returns></returns>
    public NP_DataSupportor GetNPTreeDataDeepCopyBBValuesOnly(long rootNodeId)
    {
        NP_DataSupportor result = new NP_DataSupportor();
        var source = GetNPTreeData(rootNodeId);
        result.BuffNodeDataDic = source.BuffNodeDataDic;
        result.NPBehaveTreeDataId = source.NPBehaveTreeDataId;
        result.NP_DataSupportorDic = source.NP_DataSupportorDic;
        result.NP_BBValueManager = new Dictionary<string, ANP_BBValue>();
        foreach (KeyValuePair<string, ANP_BBValue> valuePair in source.NP_BBValueManager)
        {
            result.NP_BBValueManager[valuePair.Key] = valuePair.Value.DeepCopy();
        }

        return result;
    }
    
}