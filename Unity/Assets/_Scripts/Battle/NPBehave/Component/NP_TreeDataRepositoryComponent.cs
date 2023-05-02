using System.Collections.Generic;
using Framework;
using NPBehave;
using UnityEngine;

public class NP_TreeDataRepositoryComponent : Entity, IAwakeSystem
{
    /// <summary>
    /// 运行时的行为树仓库，注意，一定不能对这些数据做修改
    /// </summary>
    public Dictionary<long, NP_DataSupportor> NpRuntimeSkillTreesDatas = new Dictionary<long, NP_DataSupportor>();

    public NP_DataSupportor GetNP_SkillTreeData(long id)
    {
        if (NpRuntimeSkillTreesDatas.TryGetValue(id, out var dataSupportor)) return dataSupportor;
        var skillCanvasConfig = SkillCanvasDataFactory.Instance.GetByNpDataId(id);
        TextAsset textAsset =
            ResComponent.Instance.LoadAsset<TextAsset>(skillCanvasConfig.SkillConfigPath);

        if (textAsset.bytes.Length == 0) Log.Msg("没有读取到文件");

        dataSupportor = SerializeHelper.Deserialize<NP_DataSupportor>(textAsset.text);
        NpRuntimeSkillTreesDatas[id] = dataSupportor;
        return dataSupportor;
    }

    /// <summary>
    /// 获取一棵树的所有数据（仅深拷贝黑板数据内容，而忽略例如BuffNodeDataDic的数据内容）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public NP_DataSupportor GetNPSkillTreeDataDeepCopyBBValuesOnly(long id)
    {
        NP_DataSupportor result = new NP_DataSupportor();
        var source = GetNP_SkillTreeData(id);
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
    
    /// <summary>
    /// 运行时的行为树仓库，注意，一定不能对这些数据做修改
    /// </summary>
    public Dictionary<long, NP_DataSupportor> NpRuntimeTreesDatas = new Dictionary<long, NP_DataSupportor>();

    public NP_DataSupportor GetNP_TreeData(long id)
    {
        if (NpRuntimeTreesDatas.TryGetValue(id, out var dataSupportor)) return dataSupportor;
        var skillCanvasConfig = AICanvasConfigFactory.Instance.GetByNpDataId(id);
        TextAsset textAsset =
            ResComponent.Instance.LoadAsset<TextAsset>(skillCanvasConfig.ConfigPath);

        if (textAsset.bytes.Length == 0) Log.Msg("没有读取到文件");

        dataSupportor = SerializeHelper.Deserialize<NP_DataSupportor>(textAsset.bytes);
        NpRuntimeTreesDatas[id] = dataSupportor;
        return dataSupportor;
    }

    /// <summary>
    /// 获取一棵树的所有数据（仅深拷贝黑板数据内容，而忽略例如BuffNodeDataDic的数据内容）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public NP_DataSupportor GetNPTreeDataDeepCopyBBValuesOnly(long id)
    {
        NP_DataSupportor result = new NP_DataSupportor();
        var source = GetNP_TreeData(id);
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

    public void Awake()
    {
    }
}