using System;
using Framework;
using Sirenix.OdinInspector;
using Unity.Mathematics;

[Title("移动到技能所找的目标", TitleAlignment = TitleAlignments.Centered)]
public class WaitMoveToSkillTarget : SkillBaseAction
{
    [LabelText("目标阵营")]
    public RoleCast RoleCast;

    [LabelText("目标标签")]
    public RoleTag RoleTag;

    public override Action GetActionToBeDone()
    {
        return MoveToTarget;
    }

    private void MoveToTarget()
    {
        var targetUnit = BelongToUnit.GetComponent<FindTargetComponent>().GetMinDisTargetUnit(out float range);
        if (targetUnit != null)
        {
            var dis = math.distance(targetUnit.Position, BelongToUnit.Position);
            if (dis > range)
            {
                float3 target = targetUnit.Position +
                                math.normalize(BelongToUnit.Position - targetUnit.Position) * range;
                BelongToUnit.GetComponent<MoveComponent>().MoveTo(target,
                    BelongToUnit.GetComponent<NumericComponent>().GetByKey(NumericType.Speed));
            }
        }
        else
        {
            if (BelongToUnit.GetComponent<FindTargetComponent>()
                .FindTarget(RoleCast, RoleTag, float.MaxValue, out var unitId, false))
            {
                targetUnit = BelongToUnit.DomainScene().GetComponent<UnitComponent>().Get(unitId);
                BelongToUnit.GetComponent<MoveComponent>().MoveTo(targetUnit.Position,
                    BelongToUnit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed));
            }
        }
    }
}