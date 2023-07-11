using System.Collections.Generic;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;
using UnityEngine;

public class NP_RunAnotherTreeData : NP_NodeDataBase , ISerializationCallbackReceiver
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

        foreach (var value in GetValue.Dic)
        {
            value.Value.configId = ConfigId;
        }
    }
    
    private void EmptyFunc()
    {
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        if (PassValue == null) PassValue = new SerializeDictionary<IBlackboardOrValue, NP_OtherTreeBBKeyData>();
        if (GetValue == null) GetValue = new SerializeDictionary<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData>();
        PassValue.CustomAddFunc += CustomAddPassValue;
        GetValue.CustomAddFunc += CustomAddGetValue;
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
}