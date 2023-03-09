using Sirenix.OdinInspector;
using UnityEngine;

public class SustainDamageBuffData : BuffDataBase
{
    [BoxGroup("自定义项")] [LabelText("伤害类型")] public BuffDamageTypes BuffDamageTypes;

    [BoxGroup("自定义项")] [LabelText("预伤害修正")]
    public float DamageFix = 1.0f;

    [Tooltip("1000为1s")] [BoxGroup("自定义项")] [LabelText("作用间隔")]
    public long WorkInternal = 0;
}