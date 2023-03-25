using Sirenix.OdinInspector;

[System.Flags]
public enum SkillDamageTypes
{
    /// <summary>
    /// 无伤害
    /// </summary>
    None = 1 << 1,

    [LabelText("物理伤害")] Physical = 1 << 5,

    [LabelText("真实伤害")] Real = 1 << 7,
    
    [LabelText("魔法伤害")] Magic = 1 << 8,
}