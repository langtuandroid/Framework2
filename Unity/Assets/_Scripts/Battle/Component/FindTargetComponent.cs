using System;
using System.Collections.Generic;
using Framework;

public class FindTargetComponent : Entity
{
    public void FindTarget(Action<long> findCb, RoleCast roleCast, RoleTag tag)
    {
        UnitComponent unitComponent = Domain.GetComponent<UnitComponent>();
        var selfRoleCast = GetComponent<B2D_RoleCastComponent>();
        foreach (var unit in unitComponent.idUnits.Values)
        {
            if (selfRoleCast.GetRoleCastToTarget(unit) == roleCast && tag.Contains(unit.GetComponent<B2D_RoleCastComponent>().RoleTag))
            {
                findCb(unit.InstanceId);
                break;
            }
        }
    }
    
    public void FindTargets(Action<RecyclableList<long>> findCb, RoleCast roleCast, RoleTag tag)
    {
        RecyclableList<long> result = RecyclableList<long>.Create();
        UnitComponent unitComponent = Domain.GetComponent<UnitComponent>();
        var selfRoleCast = GetComponent<B2D_RoleCastComponent>();
        foreach (var unit in unitComponent.idUnits.Values)
        {
            if (selfRoleCast.GetRoleCastToTarget(unit) == roleCast &&
                tag.Contains(unit.GetComponent<B2D_RoleCastComponent>().RoleTag))
            {
                result.Add(unit.InstanceId);
            }
        }

        findCb(result);
    }
}