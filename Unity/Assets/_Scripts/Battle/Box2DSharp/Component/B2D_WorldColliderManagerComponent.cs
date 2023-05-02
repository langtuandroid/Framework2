using System.Collections.Generic;
using Framework;

/// <summary>
/// 用于管理一个物理世界中所有的碰撞实体
/// </summary>
public class B2D_WorldColliderManagerComponent : Entity
{
    /// <summary>
    /// 用于管理碰撞实体
    /// </summary>
    private Dictionary<long, Unit> m_AllColliderUnits = new Dictionary<long, Unit>();

    public void AddColliderUnit(Unit b2SCollider)
    {
        this.m_AllColliderUnits.Add(b2SCollider.Id, b2SCollider);
    }

    public void RemoveColliderUnit(long id)
    {
        if (this.m_AllColliderUnits.ContainsKey(id))
        {
            this.m_AllColliderUnits.Remove(id);
        }
        else
        {
            Log.Error($"所要移除的碰撞实体ID：{id}不存在");
        }
    }

    public Unit GetColliderUnit(long id)
    {
        if (this.m_AllColliderUnits.TryGetValue(id, out var b2SColliderEntity))
        {
            return b2SColliderEntity;
        }
        else
        {
            Log.Error($"所请求的碰撞实体ID：{id}不存在");
            return null;
        }
    }
}