using System.Collections.Generic;
using Framework;
using Newtonsoft.Json;
using NPBehave;
using UnityEngine;

public class NP_TreeDataRepositoryComponent : Entity, IAwake
{
    /// <summary>
    /// 运行时的行为树仓库，注意，一定不能对这些数据做修改
    /// </summary>
    public Dictionary<long, NP_DataSupportor> NpRuntimeTreesDatas = new Dictionary<long, NP_DataSupportor>();

    private NP_DataSupportor GetOrLoadTreeData(long id)
    {
        if (NpRuntimeTreesDatas.TryGetValue(id, out var dataSupportor)) return dataSupportor;
        var skillCanvasConfig = SkillCanvasDataFactory.Instance.GetByNpDataId(id);
        TextAsset textAsset =
            ResComponent.Instance.LoadAsset<TextAsset>(skillCanvasConfig.SkillConfigName);

        if (textAsset.bytes.Length == 0) Log.Msg("没有读取到文件");

        dataSupportor = SerializeHelper.Deserialize<NP_DataSupportor>(textAsset.bytes);
        NpRuntimeTreesDatas[id] = dataSupportor;
        return dataSupportor;
    }

    /// <summary>
    /// 获取一棵树的所有数据（默认形式）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public NP_DataSupportor GetNP_TreeData(long id)
    {
        return GetOrLoadTreeData(id);
    }

    /// <summary>
    /// 获取一棵树的所有数据（仅深拷贝黑板数据内容，而忽略例如BuffNodeDataDic的数据内容）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public NP_DataSupportor GetNPTreeDataDeepCopyBBValuesOnly(long id)
    {
        NP_DataSupportor result = new NP_DataSupportor();
        var source = GetOrLoadTreeData(id);
        result.BuffNodeDataDic = source.BuffNodeDataDic;
        result.NPBehaveTreeDataId = source.NPBehaveTreeDataId;
        result.NP_DataSupportorDic = source.NP_DataSupportorDic;
        result.NP_BBValueManager = new Dictionary<string, ANP_BBValue>();
        foreach (KeyValuePair<string,ANP_BBValue> valuePair in source.NP_BBValueManager)
        {
            result.NP_BBValueManager[valuePair.Key] = valuePair.Value.DeepCopy();
        }
        return result;
    }

    /// <summary>
    /// 获取一棵树的所有数据（通过深拷贝形式）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public NP_DataSupportor GetNP_TreeData_DeepCopy(long id)
    {
        if (NpRuntimeTreesDatas.ContainsKey(id))
        {
            return NpRuntimeTreesDatas[id].DeepCopy();
        }

        Log.Error($"请求的行为树id不存在，id为{id}");
        return null;
    }
}

public class NP_RuntimeTreeRepositoryAwakeSystem : AwakeSystem<NP_TreeDataRepositoryComponent>
{
    protected override void Awake(NP_TreeDataRepositoryComponent self)
    {
        foreach (var skillCanvasConfig in SkillCanvasDataFactory.Instance.GetAll())
        {
            TextAsset textAsset =
                ResComponent.Instance.LoadAsset<TextAsset>(skillCanvasConfig.Value.SkillConfigName);

            if (textAsset.bytes.Length == 0) Log.Msg("没有读取到文件");
            try
            {
                var MnNpDataSupportor = SerializeHelper.Deserialize<NP_DataSupportor>(textAsset.bytes);

                Log.Msg($"反序列化行为树:{skillCanvasConfig.Value.SkillConfigName}完成");

                self.NpRuntimeTreesDatas.Add(MnNpDataSupportor.NPBehaveTreeDataId, MnNpDataSupportor);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}