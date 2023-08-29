using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("Buff数据块", TitleAlignment = TitleAlignments.Centered)]
[HideLabel]
[HideReferenceObjectPicker]
public class BuffDataBase
{
    /// <summary>
    /// 用于区分Buff，每个Buff Id都是独一无二的
    /// 因为我们不能，也不应该关心具体Buff的Id，所以这里直接自动生成
    /// </summary>
    [HideInInspector] [LabelText("Buff的Id")] [BoxGroup("必填项")]
    public long BuffId;

    [LabelText("Buff的添加目标")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public BuffTargetTypes BuffTargetTypes ;

    [LabelText("Buff的基本特征")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public BuffBaseType BuffBaseType ;

    [LabelText("Buff类型")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public BuffWorkTypes BuffWorkType ;

    [BoxGroup("选填项")]
    [LabelText("Buff是否状态栏可见")]
    [ShowInInspector]
    public bool Base_isVisualable ;

    [ShowIf("Base_isVisualable")]
    [LabelText("Buff图标的名称")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public string SpritePath ;

    [Tooltip("是否可以叠加(不能叠加就刷新，叠加满也刷新)")]
    [LabelText("是否叠加")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public bool CanOverlay ;

    [ShowIf("CanOverlay")]
    [LabelText("叠加层数")]
    [MinValue(1)]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public int TargetOverlay = 1;

    [ShowIf("CanOverlay")]
    [LabelText("最大叠加数")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public int MaxOverlay ;

    [LabelText("要抛出的事件ID，如果有的话")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public List<string> EventIds = new List<string>();

    [LabelText("Buff持续时间")]
    [Tooltip("-1代表永久,0代表只执行一次,单位ms")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public long SustainTime ;

    [LabelText("Buff基础数值影响者")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public BuffBaseDataEffectTypes BaseBuffBaseDataEffectTypes ;

    [LabelText("基础数值")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public int BasicValue;

    [Tooltip("具体的加成(可能会一个效果多种加成方式)，例如法强加成")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public Dictionary<BuffAdditionTypes, float> AdditionValue =
        new Dictionary<BuffAdditionTypes, float>();

    [LabelText("修改的属性")]
    [ShowInInspector]
    [ShowIf("@((BuffWorkType & BuffWorkTypes.ChangeProp) == BuffWorkTypes.ChangeProp)")]
    [BoxGroup("选填项")]
    [ValueDropdown("@NumericType.Str2TypeDoubleMap.Keys")]
    public string PropTypeStr = nameof(NumericType.None);

    public int PropType => NumericType.Str2TypeDoubleMap.GetValueByKey(PropTypeStr);


}