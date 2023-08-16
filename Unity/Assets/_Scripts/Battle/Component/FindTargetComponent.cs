using System;
using System.Collections.Generic;
using Framework;
using Unity.Mathematics;

public class FindTargetComponent : Entity, IAwakeSystem, IDestroySystem, IUpdateSystem
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

    private Dictionary<RoleKey, EntityRef<Unit>> cacheTargets = new();
    private Unit selfUnit;

    public void Awake()
    {
        selfUnit = GetParent<Unit>();
    }

    public bool FindTarget(RoleCast roleCast, RoleTag tag, float range, out long result, bool cache = true)
    {
        RoleKey key = new(roleCast, tag, range);
        result = 0;
        foreach (var cacheTarget in cacheTargets)
        {
            if (cacheTarget.Value.IsDisposed)
            {
                continue;
            }

            if (cacheTarget.Key.IsRoleEqual(key))
            {
                var dis = math.distance(cacheTarget.Value.Entity.Position, selfUnit.Position);
                if (dis < range)
                {
                    result = cacheTarget.Value.Entity.Id;
                }
            }
        }

        if (result != 0)
        {
            return true;
        }

        if (cacheTargets.TryGetValue(key, out var targetUnit))
        {
            if (math.distance(targetUnit.Entity.Position, selfUnit.Position) < range)
            {
                result = targetUnit.Entity.Id;
                return true;
            }
        }

        UnitComponent unitComponent = Domain.GetComponent<UnitComponent>();
        RoleCastComponent selfRoleCast = parent.GetComponent<RoleCastComponent>();
        foreach (Unit unit in unitComponent.idUnits.Values)
        {
            if (selfRoleCast.GetRoleCastToTarget(unit) == roleCast &&
                tag.Contains(unit.GetComponent<RoleCastComponent>().RoleTag))
            {
                if (math.distance(selfUnit.Position, unit.Position) < range)
                {
                    result = unit.Id;
                    if (cache)
                        cacheTargets[key] = new EntityRef<Unit>(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private RecyclableList<RoleKey> needRemoveKey = RecyclableList<RoleKey>.Create();

    public Unit GetMinDisTargetUnit(out float range)
    {
        float minDis = float.MaxValue;
        Unit result = null;
        range = 0;
        foreach (var cacheTarget in cacheTargets)
        {
            if (cacheTarget.Value.IsDisposed)
            {
                continue;
            }

            var dis = math.distance(selfUnit.Position, cacheTarget.Value.Entity.Position);
            if (dis < minDis)
            {
                minDis = dis;
                result = cacheTarget.Value.Entity;
                range = cacheTarget.Key.AttackRange;
            }
        }

        return result;
    }

    private void CheckNeedRemoveUnit()
    {
        needRemoveKey.Clear();
        foreach (var cacheTarget in cacheTargets)
        {
            if (cacheTarget.Value.IsDisposed)
            {
                needRemoveKey.Add(cacheTarget.Key);
                continue;
            }

            var dis = math.distance(cacheTarget.Value.Entity.Position, selfUnit.Position);
            if (dis > cacheTarget.Key.AttackRange)
            {
                needRemoveKey.Add(cacheTarget.Key);
            }
        }

        foreach (RoleKey roleKey in needRemoveKey)
        {
            cacheTargets.Remove(roleKey);
        }
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

    public void Update(float deltaTime)
    {
        CheckNeedRemoveUnit();
    }
}