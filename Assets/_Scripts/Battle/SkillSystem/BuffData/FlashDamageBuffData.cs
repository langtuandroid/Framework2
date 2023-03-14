using Sirenix.OdinInspector;

public class FlashDamageBuffData : BuffDataBase
{
    [LabelText("伤害类型")]
    [BoxGroup("自定义项")]
    [ShowInInspector]
    public SkillDamageTypes DamageType { get; set; }

    [BoxGroup("自定义项")] [LabelText("伤害附带的信息")]
    public string CustomData;

    [BoxGroup("自定义项")] [LabelText("预伤害修正")]
    public float DamageFix = 1.0f;
}