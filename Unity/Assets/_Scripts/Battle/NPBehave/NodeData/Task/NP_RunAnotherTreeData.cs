﻿using System.Collections.Generic;
using System.IO;
using Framework;
using GraphProcessor;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_RunAnotherTreeData : NP_NodeDataBase, IGraphNodeDeserialize
{
    public override NodeType BelongNodeType => NodeType.Tree;

    [OnValueChanged(nameof(OnConfigIdChanged))]
    [Required] [LabelText("对应配置表id")] public int ConfigId;

    [LabelText("传递过去的数据")] public SerializeDictionary<IBlackboardOrValue, NP_OtherTreeBBKeyData> PassValue = new();

    [LabelText("从那边拿到的数据")] public SerializeDictionary<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData> GetValue = new();

    private Task runTask;
    
    public override ExtraBehave CreateTree(Unit unit, NP_RuntimeTree runtimeTree)
    {
        ExtraBehave behave = new ExtraBehave();
        behave.PassValue = PassValue.Dic;
        behave.GetValue = GetValue.Dic;
        behave.Root = NP_RuntimeTreeFactory.LoadExtraTree(unit, runtimeTree, ConfigId, behave);
        return behave;
    }

    public override Node NP_GetNode()
    {
        if (runTask == null)
        {
            runTask = new Action(EmptyFunc);
        }

        return runTask;
    }

    private void OnConfigIdChanged()
    {
        foreach (var value in PassValue.Dic)
        {
            value.Value.configId = ConfigId;
        }

        AddRequirePassData();

        foreach (var value in GetValue.Dic)
        {
            value.Value.configId = ConfigId;
        }
    }

    private NP_DataSupportor dataSupportor;
    private void AddRequirePassData()
    {
        if (dataSupportor == null || dataSupportor.ExcelId != ConfigId)
        {
            var data = BehaveConfigFactory.Instance.Get(ConfigId);
            if (data != null)
            {
                this.dataSupportor =
                    SerializeHelper.Deserialize<NP_DataSupportor>(File.ReadAllText(data.ConfigPath));
            }
        }

        if (dataSupportor != null)
        {
            foreach (var item in dataSupportor.NP_BBValueManager)
            {
                if (item.Value.Required)
                {
                    bool hasAdd = false;
                    foreach (var key in PassValue.Dic.Values)
                    {
                        if (key.BBKey == item.Key)
                        {
                            hasAdd = true;
                            break;
                        }
                    }

                    if (!hasAdd)
                    {
                        var keyData = new NP_OtherTreeBBKeyData();
                        keyData.configId = ConfigId;
                        keyData.BBKey = item.Key;
                        PassValue.AddData(NP_BBValueHelper.GetBlackboardOrValueByBBValue(item.Value), keyData);
                    }
                }
            }
        }
    }
    
    private void EmptyFunc()
    {
    }

    private void CustomAddGetValue(List<SerializeDicKeyValue<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData>> obj)
    {
        var otherKeyData = new NP_OtherTreeBBKeyData();
        otherKeyData.configId = ConfigId;
        obj.Add(new SerializeDicKeyValue<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData>(new NP_BlackBoardKeyData(),otherKeyData));
    }

    private void CustomAddPassValue(List<SerializeDicKeyValue<IBlackboardOrValue, NP_OtherTreeBBKeyData>> obj)
    {
        var otherKeyData = new NP_OtherTreeBBKeyData();
        otherKeyData.configId = ConfigId;
        obj.Add(new SerializeDicKeyValue<IBlackboardOrValue, NP_OtherTreeBBKeyData>(null, otherKeyData));
    }

    public void OnNodeDeserialize()
    {
        if (PassValue == null) PassValue = new SerializeDictionary<IBlackboardOrValue, NP_OtherTreeBBKeyData>();
        if (GetValue == null) GetValue = new SerializeDictionary<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData>();
        OnConfigIdChanged();
        PassValue.CustomAddFunc -= CustomAddPassValue;
        GetValue.CustomAddFunc -= CustomAddGetValue;
        PassValue.CustomAddFunc += CustomAddPassValue;
        GetValue.CustomAddFunc += CustomAddGetValue;
    }
}