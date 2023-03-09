using Sirenix.OdinInspector;

public class ChangePropertyBuffData : BuffDataBase
{
    /// <summary>
    /// 将要被添加的值
    /// </summary>
    [BoxGroup("自定义项")] [LabelText("将要被添加的值")]
    public float TheValueWillBeAdded = 0;
}