using Framework;

public enum RoleCast
{
    /// <summary>
    /// 友善的
    /// </summary>
    Friendly,

    /// <summary>
    /// 敌对的
    /// </summary>
    Adverse,

    /// <summary>
    /// 中立的
    /// </summary>
    Neutral
}

[System.Flags]
public enum RoleCamp
{
    red = 1 << 1,
    bule = 1 << 2,
}

public enum RoleTag
{
    Hero,
    Soldier,
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