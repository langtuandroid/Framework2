using Sirenix.OdinInspector;

/// <summary>
/// Buff的奏效的表现
/// </summary>
[System.Flags]
public enum BuffWorkType
{
    [LabelText("无")] None = 1,

    [LabelText("击退")] Repulse = 1 << 1,

    [LabelText("沉默")] Silence = 1 << 2,

    [LabelText("眩晕")] Dizziness = 1 << 3,

    [LabelText("击飞")] StrikeToFly = 1 << 4,

    [LabelText("暴击")] CriticalStrike = 1 << 6,

    [LabelText("无敌")] Invincible = 1 << 11,

    [LabelText("禁锢")] Shackle = 1 << 12,

    [LabelText("隐身")] Invisible = 1 << 13,

    [LabelText("斩杀")] Kill = 1 << 14,
    
    [LabelText("更改属性")] ChangeProp = 1 << 15,
}