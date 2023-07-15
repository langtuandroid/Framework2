using System.Collections.Generic;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;

[HideReferenceObjectPicker]
[LabelText("碰撞数据")]
public class DefaultColliderNode
{
    [LabelText("是否只能触发某一个目标")] public BlackboardOrValue_Bool IsOnlyOneTarget = new BlackboardOrValue_Bool(false);

    [HideIf("@IsOnlyOneTarget.GetEditorValue()")] [LabelText("碰撞的标签")]
    public RoleTag RoleTag = RoleTag.Hero | RoleTag.Soldier;

    [HideIf("@IsOnlyOneTarget.GetEditorValue()")] [LabelText("碰撞的阵营")]
    public RoleCast RoleCast = RoleCast.Adverse;

    [LabelText("是否碰撞到的字典key")] public NP_BlackBoardRelationData<bool> HasHitKey = new();

    [HideIf("@IsOnlyOneTarget.GetEditorValue()")] [LabelText("碰撞到的所有物体字典key")]
    public NP_BlackBoardRelationData<List<long>> HitUnitListKey = new();

    [HideIf("@IsOnlyOneTarget.GetEditorValue()")] [LabelText("碰撞到的物体字典key")]
    public NP_BlackBoardRelationData<long> HitUnitKey = new();

    [LabelText("只能触发的目标")] [ShowIf("@IsOnlyOneTarget.GetEditorValue()")]
    public BlackboardOrValue_Long OnlyTriggerTarget = new();

    public DefaultColliderData ToColliderData(Blackboard blackboard)
    {
        return new DefaultColliderData(blackboard, RoleTag, RoleCast,
            HasHitKey.BBKey, HitUnitKey.BBKey, HitUnitListKey.BBKey, OnlyTriggerTarget.GetValue(blackboard));
    }
}