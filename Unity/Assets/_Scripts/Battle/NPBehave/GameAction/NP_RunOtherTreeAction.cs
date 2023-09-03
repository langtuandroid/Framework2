using System.Collections.Generic;
using System.IO;
using Framework;
using GraphProcessor;
using Sirenix.OdinInspector;

public class NP_RunOtherTreeAction : NP_ClassForStoreAction, IGraphNodeDeserialize
{
    [LabelText("目标")]
    public NP_BlackBoardRelationData<long> TargetUnitId = new();

    [LabelText("是技能吗")]
    public bool IsSkill;

    [OnValueChanged(nameof(OnConfigIdChanged))]
    [Required]
    [LabelText("对应配置表id")]
    public int ConfigId;

    [LabelText("传递过去的数据")]
    public SerializeDictionary<IBlackboardOrValue, NP_OtherTreeBBKeyData> PassValue = new();

    public override System.Action GetActionToBeDone()
    {
        return Run;
    }

    private void Run()
    {
        NP_RuntimeTree tree = null;
        Unit targetUnit = this.BelongToUnit.DomainScene().GetComponent<UnitComponent>()
            .Get(TargetUnitId.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard()));
        if (IsSkill)
        {
            tree = NP_RuntimeTreeFactory.CreateSkillRuntimeTree(BelongToUnit, ConfigId);
        }
        else
        {
            tree = NP_RuntimeTreeFactory.CreateBehaveRuntimeTree(BelongToUnit, ConfigId);
        }

        foreach (var passItem in PassValue.Dic)
        {
            var value = passItem.Key.GetObjValue(BelongtoRuntimeTree.GetBlackboard());
            tree.GetBlackboard().Set(passItem.Value.BBKey,
                NP_BBValueHelper.AutoCreateNPBBValueFromTValue(value, value.GetType()));
        }

        tree.Start();
    }

#if UNITY_EDITOR
    private void OnConfigIdChanged()
    {
        foreach (var value in PassValue.Dic)
        {
            value.Value.Refresh(ConfigId, IsSkill);
        }

        AddRequirePassData();
    }

    private NP_DataSupportor dataSupportor;

    private void AddRequirePassData()
    {
        if (dataSupportor == null || dataSupportor.ExcelId != ConfigId)
        {
            string configPath = "";
            if (IsSkill)
            {
                var data = SkillBehaveConfigFactory.Instance.Get(ConfigId);
                configPath = data?.ConfigPath;
            }
            else
            {
                var data = BehaveConfigFactory.Instance.Get(ConfigId);
                configPath = data?.ConfigPath;
            }

            if (!string.IsNullOrEmpty(configPath))
            {
                this.dataSupportor =
                    SerializeHelper.Deserialize<NP_DataSupportor>(File.ReadAllText(configPath));
            }
        }
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
        OnConfigIdChanged();
        PassValue.CustomAddFunc -= CustomAddPassValue;
        PassValue.CustomAddFunc += CustomAddPassValue;
    }
#endif
}