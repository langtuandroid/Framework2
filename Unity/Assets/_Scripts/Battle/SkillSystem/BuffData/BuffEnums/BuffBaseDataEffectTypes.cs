using Sirenix.OdinInspector;

[System.Flags]
public enum BuffBaseDataEffectTypes
{
    [LabelText("来自英雄等级的加成")] FromHeroLevel = 1 << 1,

    [LabelText("来自技能等级的加成")] FromSkillLevel = 1 << 2,

    [LabelText("已损失生命值")] FromHasLostLifeValue = 1 << 3,

    [LabelText("当前Buff叠加层数")] FromCurrentOverlay = 1 << 4,
}