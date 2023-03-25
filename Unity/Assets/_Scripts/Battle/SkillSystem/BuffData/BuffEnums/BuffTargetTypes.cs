using System;
using Sirenix.OdinInspector;

[Flags]
public enum BuffTargetTypes
{
    /// <summary>
    /// 自己
    /// </summary>
    [LabelText("自己")] Self = 1 << 1,

    /// <summary>
    /// 别人
    /// </summary>
    [LabelText("别人")] Others = 1 << 2,
}