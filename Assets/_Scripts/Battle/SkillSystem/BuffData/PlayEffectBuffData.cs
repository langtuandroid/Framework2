using Sirenix.OdinInspector;

public class PlayEffectBuffData : BuffDataBase
{
    [BoxGroup("自定义项")] [LabelText("要播放的特效名称")]
    public string EffectName;

    [BoxGroup("自定义项")] [LabelText("是否会根据当前Buff层数而改变")]
    public bool CanChangeNameByCurrentOverlay = false;

    /// <summary>
    /// 是否跟随归属的Unit，默认是跟随的，不跟随需要指定一个地点
    /// </summary>
    [BoxGroup("自定义项")] [LabelText("是否跟随归属的Unit")]
    public bool FollowUnit = true;

    /// <summary>
    /// 特效将要粘贴到的位置
    /// </summary>
    [BoxGroup("自定义项")] [LabelText("生成的位置")]
    public string Pos;
}