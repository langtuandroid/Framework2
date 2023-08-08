using System;
using Sirenix.OdinInspector;
using Action = NPBehave.Action;

public class NP_StartSkillAction : NP_ClassForStoreAction
{
    [LabelText("技能优先级")]
    public int SkillPriority;

    private SkillCanvasManagerComponent skillCanvasManagerComponent;
    private int skillPriorityTimer;

    public override Func<bool, Action.Result> GetFunc2ToBeDone()
    {
        return CanRunSkill;
    }

    private Action.Result CanRunSkill(bool isCancel)
    {
        if (skillCanvasManagerComponent == null)
        {
            skillCanvasManagerComponent = BelongToUnit.GetComponent<SkillCanvasManagerComponent>();
            skillPriorityTimer = SkillPriority;
        }

        if (skillCanvasManagerComponent.IsSkillRunning)
        {
            return Action.Result.PROGRESS;
        }

        if (skillPriorityTimer <= 0)
        {
            skillPriorityTimer = SkillPriority;
            return Action.Result.SUCCESS;
        }

        skillPriorityTimer--;
        return Action.Result.PROGRESS;
    }
}