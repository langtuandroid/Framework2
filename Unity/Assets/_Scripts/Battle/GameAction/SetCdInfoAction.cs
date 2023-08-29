using Framework;

public class SetCdInfoAction : SkillBaseAction
{
    /// <summary>
    /// 设置cd，第二个参数是cd的相乘系数
    /// </summary>
    public void SetCdInfo(int skillId, float cdMult = 1)
    {
        Unit unit = BelongToUnit;
        CDComponent cdComponent = unit.Domain.GetComponent<CDComponent>();
        int skillLevel = unit.GetComponent<SkillManagerComponent>().GetSkillLevel(skillId);
        var skillConfig = SkillConfig.GetById(skillId);
        float cd = skillConfig.Cd[skillLevel];
        cdComponent.SetCD(unit.Id, skillConfig.name + unit.Id, cd, cd * cdMult);
    }

    public override void Clear()
    {
    }
}