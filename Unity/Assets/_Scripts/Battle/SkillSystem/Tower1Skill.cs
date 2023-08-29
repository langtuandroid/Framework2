using Framework;
using NPBehave;

public class Tower1Skill : Entity
{
    public void Run()
    {
        var unit = GetParent<Unit>();
        using var setCd = SkillBaseAction.Create<SetCdInfoAction>(unit);
        setCd.SetCdInfo(1);
    }
}