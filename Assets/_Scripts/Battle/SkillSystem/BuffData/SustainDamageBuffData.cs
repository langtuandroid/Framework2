using Sirenix.OdinInspector;
using UnityEngine;

public class SustainDamageBuffData : BuffWithValueData
{
    [Tooltip("1000为1s")] [BoxGroup("自定义项")] [LabelText("作用间隔")]
    public long WorkInternal = 0;
}