using System;
using Framework;
using Sirenix.OdinInspector;

public class EndSkillAction : SkillBaseAction
{
    [LabelText("技能描述结点Id")]
    public VTD_Id DataId = new();

    public override Action GetActionToBeDone()
    {
        return EndSkill;
    }

    private void EndSkill()
    {
        SkillManagerComponent skillManagerComponent =
            BelongToUnit.GetComponent<SkillManagerComponent>();
        SkillDesNodeData skillDesNodeData =
            (SkillDesNodeData)BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[DataId.Value];
        skillManagerComponent.SkillEnd(skillDesNodeData.SkillId);
    }
}