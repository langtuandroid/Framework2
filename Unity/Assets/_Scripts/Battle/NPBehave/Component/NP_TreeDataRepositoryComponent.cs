using System.Collections.Generic;
using Framework;
using NPBehave;
using UnityEngine;
using UnityEngine.UI;

public class NP_TreeDataRepositoryComponent : Entity, IAwakeSystem
{
    /// <summary>
    /// 运行时的行为树仓库，注意，一定不能对这些数据做修改
    /// </summary>
    private Dictionary<string, NP_DataSupportor> BehaveConfigPath2ConfigContent =
        new Dictionary<string, NP_DataSupportor>();


    public NP_DataSupportor GetNPTreeData(string treeConfigPath)
    {
        if (!BehaveConfigPath2ConfigContent.TryGetValue(treeConfigPath, out var dataSupportor))
        {
            TextAsset textAsset =
                ResComponent.Instance.LoadAsset<TextAsset>(treeConfigPath);

            if (textAsset.bytes.Length == 0) Log.Msg("没有读取到文件");
            dataSupportor = SerializeHelper.Deserialize<NP_DataSupportor>(textAsset.text);
            BehaveConfigPath2ConfigContent[treeConfigPath] = dataSupportor;
        }

        return dataSupportor;
    }

    public NP_DataSupportor GetNPTreeData(long rootId)
    {
        foreach (NP_DataSupportor npDataSupportor in BehaveConfigPath2ConfigContent.Values)
        {
            if (npDataSupportor.NPBehaveTreeDataId == rootId)
            {
                return npDataSupportor;
            }
        }

        throw new Exception("不对啊");
    }
    
    /// <summary>
    /// 获取一棵树的所有数据（仅深拷贝黑板数据内容，而忽略例如BuffNodeDataDic的数据内容）
    /// </summary>
    /// <param name="rootNodeId"></param>
    /// <returns></returns>
    public NP_DataSupportor GetNPTreeDataDeepCopyBBValuesOnly(string treeConfigPath)
    {
        NP_DataSupportor result = new NP_DataSupportor();
        var source = GetNPTreeData(treeConfigPath);
        result.ExcelId = source.ExcelId;
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