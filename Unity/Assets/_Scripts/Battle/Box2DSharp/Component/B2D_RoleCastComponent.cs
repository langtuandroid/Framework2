using System;
using Framework;
using Sirenix.OdinInspector;

[Flags]
public enum RoleCast
{
    /// <summary>
    /// 友善的
    /// </summary>
    [LabelText("友方")]
    Friendly = 1 << 1,

    /// <summary>
    /// 敌对的
    /// </summary>
    [LabelText("敌对的")]
    Adverse = 1 << 2,

    /// <summary>
    /// 中立的
    /// </summary>
    [LabelText("中立的")]
    Neutral = 1 << 3
}

[Flags]
public enum RoleCamp
{
    red = 1 << 1,
    bule = 1 << 2,
}

[Flags]
public enum RoleTag
{
    Hero = 1 << 1,
    Soldier = 1 << 2,
}

public class B2D_RoleCastComponent : Entity, IAwakeSystem<RoleCamp, RoleTag>
{
    public RoleTag RoleTag;

    /// <summary>
    /// 归属阵营
    /// </summary>
    public RoleCamp RoleCamp;

    /// <summary>
    /// 获取与目标的关系
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public RoleCast GetRoleCastToTarget(Unit unit)
    {
        if (unit.GetComponent<B2D_RoleCastComponent>() == null)
        {
            return RoleCast.Friendly;
        }

        RoleCamp roleCamp = unit.GetComponent<B2D_RoleCastComponent>().RoleCamp;

        if (roleCamp == this.RoleCamp)
        {
            return RoleCast.Friendly;
        }

        if (roleCamp != this.RoleCamp)
        {
            return RoleCast.Adverse;
        }

        return RoleCast.Friendly;
    }

    public void Awake(RoleCamp a, RoleTag b)
    {
        RoleCamp = a;
        RoleTag = b;
    }
}