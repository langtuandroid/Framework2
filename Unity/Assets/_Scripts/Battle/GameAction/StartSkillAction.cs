using System;
using Framework;
using Sirenix.OdinInspector;
using Action = NPBehave.Action;

public class StartSkillAction : SkillBaseAction
{
    [LabelText("技能描述结点Id")]
    public VTD_Id DataId = new();

    [LabelText("技能优先级")]
    [InfoBox("优先级值越小，在可放技能的时候越优先")]
    [ValueDropdown(nameof(priority))]
    public int SkillPriority;

    private static int[] priority = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    private SkillManagerComponent skillManagerComponent;
    private int skillPriorityTimer;

    public override Func<bool, Action.Result> GetFunc2ToBeDone()
    {
        return CanRunSkill;
    }

    private Action.Result CanRunSkill(bool isCancel)
    {
        if (isCancel)
        {
            return Action.Result.SUCCESS;
        }

        if (skillManagerComponent == null)
        {
            skillManagerComponent = BelongToUnit.GetComponent<SkillManagerComponent>();
            skillPriorityTimer = SkillPriority;
        }

        if (skillManagerComponent.IsSkillRunning)
        {
            return Action.Result.PROGRESS;
        }

        if (skillPriorityTimer <= 0)
        {
            skillPriorityTimer = SkillPriority;
            SkillDesNodeData skillDesNodeData =
                (SkillDesNodeData)BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[DataId.Value];
            skillManagerComponent.SkillStart(skillDesNodeData.SkillId);
            return Action.Result.SUCCESS;
        }

        skillPriorityTimer--;
        return Action.Result.PROGRESS;
    }
}