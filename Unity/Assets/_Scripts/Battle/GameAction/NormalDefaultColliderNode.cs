using System.Collections.Generic;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;

[HideReferenceObjectPicker]
[HideLabel]
[BoxGroup("碰撞数据")]
public class NormalDefaultColliderNode
{
    [LabelText("是否只能触发某一个目标")]
    public bool IsOnlyOneTarget = false;

    [LabelText("只能触发的目标")]
    [ShowIf("IsOnlyOneTarget")]
    public BlackboardOrValue_Long OnlyTriggerTarget = new();

    [HideIf("IsOnlyOneTarget")]
    [LabelText("碰撞的标签")]
    public RoleTag RoleTag = RoleTag.Hero | RoleTag.Soldier;

    [HideIf("IsOnlyOneTarget")]
    [LabelText("碰撞的阵营")]
    public RoleCast RoleCast = RoleCast.Adverse;

    public NormalDefaultColliderData ToColliderData(Blackboard blackboard)
    {
        if (!IsOnlyOneTarget)
        {
            OnlyTriggerTarget.UseBlackboard = false;
            OnlyTriggerTarget.OriginValue = default;
        }

        var data = ReferencePool.Allocate<NormalDefaultColliderData>();
        data.RoleTag = RoleTag;
        data.RoleCast = RoleCast;
        data.OnlyTarget = OnlyTriggerTarget.GetValue(blackboard);
        return data;
    }
}