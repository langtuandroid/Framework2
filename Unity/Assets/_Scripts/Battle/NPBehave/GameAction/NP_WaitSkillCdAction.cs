using ET;
using Framework;
using Sirenix.OdinInspector;

public class NP_WaitSkillCdAction : NP_CalssForStoreWaitUntilAction
{
    [BoxGroup("引用数据的Id")] [LabelText("技能数据结点Id")]
    public VTD_Id DataId = new();

    private string skillName;

    protected override void OnStart()
    {
        SkillDesNodeData skillDesNodeData =
            (SkillDesNodeData)BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[DataId.Value];
        skillName = skillDesNodeData.SkillName;
    }

    protected override bool UntilFunc()
    {
        CDComponent cdComponent = BelongToUnit.Domain.GetComponent<CDComponent>();
        return cdComponent.GetCDResult(BelongToUnit.Id, skillName);
    }
}