using System;
using Sirenix.OdinInspector;

public abstract class ANP_BBValue
{
    public abstract Type NP_BBValueType { get; }

#if UNITY_EDITOR
    [LabelText("是否技能必须的值")]
    [ShowIf("@NP_BlackBoardDataManager.IsSkill")]
    public bool Required;
#endif
    /// <summary>
    /// 从另一个anpBbValue设置数据
    /// </summary>
    /// <param name="anpBbValue"></param>
    public abstract void SetValueFrom(ANP_BBValue anpBbValue);
}