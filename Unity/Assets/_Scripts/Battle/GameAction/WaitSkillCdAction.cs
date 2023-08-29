using System;
using Framework;
using Sirenix.OdinInspector;
using Action = NPBehave.Action;

public class WaitSkillCdAction : SkillBaseAction
{
    [LabelText("技能描述结点Id")]
    public VTD_Id DataId = new();

    private string cdName;
    private CDComponent cdComponent;

    public override Func<bool, Action.Result> GetFunc2ToBeDone()
    {
        return UntilFunc;
    }

    private Action.Result UntilFunc(bool isCancel)
    {
        if (isCancel)
        {
            return Action.Result.SUCCESS;
        }

        if (string.IsNullOrEmpty(cdName))
        {
            SkillDesNodeData skillDesNodeData =
                (SkillDesNodeData)BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[DataId.Value];
            cdName = skillDesNodeData.SkillName + BelongToUnit.Id;
            cdComponent = BelongToUnit.Domain.GetComponent<CDComponent>();
        }

        var cdInfo = cdComponent.GetCDData(BelongToUnit.Id, cdName);
        if (cdInfo == null)
        {
            return Action.Result.SUCCESS;
        }
        else
        {
            return cdInfo.Finish ? Action.Result.SUCCESS : Action.Result.PROGRESS;
        }
    }
}