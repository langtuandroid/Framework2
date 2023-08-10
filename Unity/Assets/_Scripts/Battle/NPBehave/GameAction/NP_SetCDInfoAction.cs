using System;
using Framework;
using Sirenix.OdinInspector;

public class NP_SetCDInfoAction : NP_ClassForStoreAction
{
    [BoxGroup("引用数据的Id")]
    [LabelText("技能数据结点Id")]
    public VTD_Id DataId = new();

    public override Action GetActionToBeDone()
    {
        return SetCdInfo;
    }

    private void SetCdInfo()
    {
        Unit unit = BelongToUnit;
        CDComponent cdComponent = unit.Domain.GetComponent<CDComponent>();
        SkillDesNodeData skillDesNodeData =
            (SkillDesNodeData)BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[DataId.Value];
        int skillLevel = unit.GetComponent<SkillCanvasManagerComponent>().GetSkillLevel(skillDesNodeData.SkillId);
        long cd = skillDesNodeData.SkillCD[
            unit.GetComponent<SkillCanvasManagerComponent>().GetSkillLevel(skillDesNodeData.SkillId)];
        cdComponent.SetCD(unit.Id, skillDesNodeData.SkillName + unit.Id, cd, cd);
    }
}