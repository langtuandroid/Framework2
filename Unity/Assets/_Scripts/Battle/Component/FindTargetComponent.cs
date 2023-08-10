using System;
using System.Collections.Generic;
using Framework;
using Unity.Mathematics;

public class FindTargetComponent : Entity, IAwakeSystem
{
    private struct RoleKey : IEquatable<RoleKey>
    {
        public RoleCast RoleCast;
        public RoleTag RoleTag;

        public RoleKey(RoleCast roleCast, RoleTag roleTag)
        {
            RoleCast = roleCast;
            RoleTag = roleTag;
        }

        public bool Equals(RoleKey other)
        {
            return RoleCast == other.RoleCast && RoleTag == other.RoleTag;
        }

        public override bool Equals(object obj)
        {
            return obj is RoleKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)RoleCast, (int)RoleTag);
        }
    }

    private Dictionary<RoleKey, Unit> cacheTargets = new();
    private Unit selfUnit;

    public void Awake()
    {
        selfUnit = GetParent<Unit>();
    }

    public bool FindTarget(RoleCast roleCast, RoleTag tag, float dis, out long result)
    {
        RoleKey key = new(roleCast, tag);
        if (cacheTargets.TryGetValue(key, out Unit targetUnit))
        {
            if (math.distance(targetUnit.Position, selfUnit.Position) < dis)
            {
                result = targetUnit.Id;
                return true;
            }
        }

        result = 0;
        UnitComponent unitComponent = Domain.GetComponent<UnitComponent>();
        RoleCastComponent selfRoleCast = parent.GetComponent<RoleCastComponent>();
        foreach (Unit unit in unitComponent.idUnits.Values)
        {
            if (selfRoleCast.GetRoleCastToTarget(unit) == roleCast &&
                tag.Contains(unit.GetComponent<RoleCastComponent>().RoleTag))
            {
                if (math.distance(selfUnit.Position, unit.Position) < dis)
                {
                    result = unit.Id;
                    cacheTargets[key] = unit;
                    return true;
                }
            }
        }

        return false;
    }

    public void FindTargets(Action<RecyclableList<long>> findCb, RoleCast roleCast, RoleTag tag)
    {
        RecyclableList<long> result = RecyclableList<long>.Create();
        UnitComponent unitComponent = Domain.GetComponent<UnitComponent>();
        RoleCastComponent selfRoleCast = GetComponent<RoleCastComponent>();
        foreach (Unit unit in unitComponent.idUnits.Values)
        {
            if (selfRoleCast.GetRoleCastToTarget(unit) == roleCast &&
                tag.Contains(unit.GetComponent<RoleCastComponent>().RoleTag))
            {
                result.Add(unit.Id);
            }
        }

        findCb(result);
    }
}