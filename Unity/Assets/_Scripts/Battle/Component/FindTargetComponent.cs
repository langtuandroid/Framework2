using System;
using System.Collections.Generic;
using Framework;
using Unity.Mathematics;
using UnityEngine;

public class FindTargetComponent : Entity, IAwakeSystem, IDestroySystem
{
    private struct RoleKey : IEquatable<RoleKey>
    {
        public RoleCast RoleCast;
        public RoleTag RoleTag;
        public float AttackRange;

        public RoleKey(RoleCast roleCast, RoleTag roleTag, float attackRange)
        {
            RoleCast = roleCast;
            RoleTag = roleTag;
            AttackRange = attackRange;
        }

        public bool Equals(RoleKey other)
        {
            return RoleCast == other.RoleCast && RoleTag == other.RoleTag && AttackRange.NearlyEqual(other.AttackRange);
        }

        public bool IsRoleEqual(RoleKey other)
        {
            return RoleCast == other.RoleCast && RoleTag == other.RoleTag;
        }

        public override bool Equals(object obj)
        {
            return obj is RoleKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)RoleCast, (int)RoleTag, (int)(AttackRange * 1000));
        }
    }

    private Unit selfUnit;

    public void Awake()
    {
        selfUnit = GetParent<Unit>();
    }

    public bool FindTarget(RoleCast roleCast, RoleTag tag, float range, out long result, bool cache = true)
    {
        result = 0;
        UnitComponent unitComponent = Domain.GetComponent<UnitComponent>();
        RoleCastComponent selfRoleCast = parent.GetComponent<RoleCastComponent>();
        float minDis = float.MaxValue;
        foreach (Unit unit in unitComponent.idUnits.Values)
        {
            if (selfRoleCast.GetRoleCastToTarget(unit) == roleCast &&
                tag.Contains(unit.GetComponent<RoleCastComponent>().RoleTag))
            {
                var dis = math.distance(selfUnit.Position, unit.Position);
                if (dis < range)
                {
                    if (dis < minDis)
                    {
                        result = unit.Id;
                        minDis = dis;
                    }
                }
            }
        }

        return result != 0;
    }

    private RecyclableList<RoleKey> needRemoveKey = RecyclableList<RoleKey>.Create();

    public Unit GetMinDisTargetUnit(out float range)
    {
        float minDis = float.MaxValue;
        Unit result = null;
        range = 0;
        return result;
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

    public void OnDestroy()
    {
        needRemoveKey.Dispose();
    }

}