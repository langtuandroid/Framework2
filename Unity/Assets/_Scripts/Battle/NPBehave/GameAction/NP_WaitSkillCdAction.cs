using System;
using ET;
using Framework;
using Sirenix.OdinInspector;

public class NP_WaitSkillCdAction : NP_ClassForStoreAction
{
    [BoxGroup("引用数据的Id")]
    [LabelText("技能数据结点Id")]
    public VTD_Id DataId = new();

    private string skillName;

    public override Func<bool, NPBehave.Action.Result> GetFunc2ToBeDone()
    {
        return UntilFunc;
    }

    private NPBehave.Action.Result UntilFunc(bool isCancel)
    {
        if (string.IsNullOrEmpty(skillName))
        {
            SkillDesNodeData skillDesNodeData =
                (SkillDesNodeData)BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[DataId.Value];
            skillName = skillDesNodeData.SkillName;
        }

        CDComponent cdComponent = BelongToUnit.Domain.GetComponent<CDComponent>();
        return cdComponent.GetCDResult(BelongToUnit.Id, skillName)
            ? NPBehave.Action.Result.SUCCESS
            : NPBehave.Action.Result.PROGRESS;
    }
}