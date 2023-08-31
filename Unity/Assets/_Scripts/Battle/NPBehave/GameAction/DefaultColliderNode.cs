using System.Collections.Generic;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;

[HideReferenceObjectPicker]
[HideLabel]
[BoxGroup("碰撞数据")]
public class DefaultColliderNode
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

    [LabelText("是否碰撞到的字典key")]
    public NP_BlackBoardRelationData<bool> HasHitKey = new();

    [HideIf("IsOnlyOneTarget")]
    [LabelText("碰撞到的所有物体字典key")]
    public NP_BlackBoardRelationData<List<long>> HitUnitListKey = new();

    [HideIf("IsOnlyOneTarget")]
    [LabelText("碰撞到的物体字典key")]
    public NP_BlackBoardRelationData<long> HitUnitKey = new();

    public DefaultColliderData ToColliderData(Blackboard blackboard)
    {
        if (!IsOnlyOneTarget)
        {
            OnlyTriggerTarget.UseBlackboard = false;
            OnlyTriggerTarget.OriginValue = default;
        }

        return new DefaultColliderData(blackboard, RoleTag, RoleCast,
            HasHitKey.BBKey, HitUnitKey.BBKey, HitUnitListKey.BBKey, OnlyTriggerTarget.GetValue(blackboard));
    }
}