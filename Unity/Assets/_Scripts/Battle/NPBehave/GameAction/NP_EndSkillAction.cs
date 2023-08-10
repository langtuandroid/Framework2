using System;
using Framework;
using Sirenix.OdinInspector;

public class NP_EndSkillAction : NP_ClassForStoreAction
{
    [LabelText("技能描述结点Id")]
    public VTD_Id DataId = new();

    public override Action GetActionToBeDone()
    {
        return EndSkill;
    }

    private void EndSkill()
    {
        SkillCanvasManagerComponent skillCanvasManagerComponent =
            BelongToUnit.GetComponent<SkillCanvasManagerComponent>();
        SkillDesNodeData skillDesNodeData =
            (SkillDesNodeData)BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[DataId.Value];
        skillCanvasManagerComponent.SkillEnd(skillDesNodeData.SkillId);
    }
}