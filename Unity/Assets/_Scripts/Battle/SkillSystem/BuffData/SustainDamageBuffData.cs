using Sirenix.OdinInspector;
using UnityEngine;

public class SustainDamageBuffData : BuffDataBase 
{

    [LabelText("伤害类型")]
    [BoxGroup("自定义项")]
    [ShowInInspector]
    public SkillDamageTypes DamageType { get; set; }

    [Tooltip("1000为1s")] [BoxGroup("自定义项")] [LabelText("作用间隔")]
    public long WorkInternal = 0;
}