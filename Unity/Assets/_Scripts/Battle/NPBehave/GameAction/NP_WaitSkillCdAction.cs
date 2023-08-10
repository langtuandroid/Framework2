using System;
using ET;
using Framework;
using Sirenix.OdinInspector;

public class NP_WaitSkillCdAction : NP_ClassForStoreAction
{
    [LabelText("技能描述结点Id")]
    public VTD_Id DataId = new();

    private string cdName;
    private CDComponent cdComponent;

    public override Func<bool, NPBehave.Action.Result> GetFunc2ToBeDone()
    {
        return UntilFunc;
    }

    private NPBehave.Action.Result UntilFunc(bool isCancel)
    {
        if (string.IsNullOrEmpty(cdName))
        {
            SkillDesNodeData skillDesNodeData =
                (SkillDesNodeData)BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[DataId.Value];
            cdName = skillDesNodeData.SkillName + BelongToUnit.Id;
            cdComponent = BelongToUnit.Domain.GetComponent<CDComponent>();
        }

        return cdComponent.GetCDResult(BelongToUnit.Id, cdName)
            ? NPBehave.Action.Result.SUCCESS
            : NPBehave.Action.Result.PROGRESS;
    }
}