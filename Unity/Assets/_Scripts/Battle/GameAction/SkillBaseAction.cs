using Framework;

public abstract class SkillBaseAction
{
    /// <summary>
    /// 归属的UnitID
    /// </summary>
    protected Unit BelongToUnit { get; private set; }

    public static T Create<T>(Unit unit) where T : SkillBaseAction
    {
        var skillAction = ReferencePool.Allocate<T>();
        skillAction.BelongToUnit = unit;
        return skillAction;
    }
}