using System.Collections.Generic;
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

    [LabelText("是技能吗")]
    [OnValueChanged(nameof(OnConfigIdChanged))]
    public bool IsSkill;

    [LabelText("传递过去的数据")] public SerializeDictionary<IBlackboardOrValue, NP_OtherTreeBBKeyData> PassValue = new();

    // [LabelText("从那边拿到的数据")] public SerializeDictionary<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData> GetValue = new();

    private NP_RuntimeTree runtimeTree;
    
    public override Node NP_GetNode()
    {
        return new Action(() => { }, nameof(NP_RunAnotherTreeData));
    }

    private void SetPassGetValue()
    {
        var blackboard = runtimeTree.GetBlackboard();
        foreach (var passItem in PassValue.Dic)
        {
            var value = passItem.Key.GetObjValue(blackboard);
            blackboard.Set(passItem.Value.BBKey,
                NP_BBValueHelper.AutoCreateNPBBValueFromTValue(value, value.GetType()));
        }

        // foreach (var getItem in GetValue.Dic)
        // {
        //     blackboard.Set(getItem.Key.BBKey, blackboard.Get(getItem.Value.BBKey));
        // }
    }

#if UNITY_EDITOR
    private void OnConfigIdChanged()
    {
        foreach (var value in PassValue.Dic)
        {
            value.Value.Refresh(ConfigId, IsSkill);
        }

        // foreach (var value in GetValue.Dic)
        // {
        //     value.Value.Refresh(ConfigId, IsSkill);
        // }
    }


    private void CustomAddGetValue(List<SerializeDicKeyValue<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData>> obj)
    {
        var otherKeyData = new NP_OtherTreeBBKeyData();
        otherKeyData.Refresh(ConfigId, IsSkill);
        obj.Add(new SerializeDicKeyValue<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData>(new NP_BlackBoardKeyData(),otherKeyData));
    }

    private void CustomAddPassValue(List<SerializeDicKeyValue<IBlackboardOrValue, NP_OtherTreeBBKeyData>> obj)
    {
        var otherKeyData = new NP_OtherTreeBBKeyData();
        otherKeyData.Refresh(ConfigId, IsSkill);
        obj.Add(new SerializeDicKeyValue<IBlackboardOrValue, NP_OtherTreeBBKeyData>(null, otherKeyData));
    }

    public void OnNodeDeserialize()
    {
        if (PassValue == null) PassValue = new SerializeDictionary<IBlackboardOrValue, NP_OtherTreeBBKeyData>();
        // if (GetValue == null) GetValue = new SerializeDictionary<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData>();
        OnConfigIdChanged();
        PassValue.CustomAddFunc -= CustomAddPassValue;
        PassValue.CustomAddFunc += CustomAddPassValue;
        // GetValue.CustomAddFunc -= CustomAddGetValue;
        // GetValue.CustomAddFunc += CustomAddGetValue;
    }
#endif
}