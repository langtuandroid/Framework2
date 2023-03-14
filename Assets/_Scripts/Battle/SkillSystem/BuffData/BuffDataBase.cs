using System.Collections.Generic;
using Framework;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("Buff数据块", TitleAlignment = TitleAlignments.Centered)]
[HideLabel]
[HideReferenceObjectPicker]
[BsonDeserializerRegister]
public class BuffDataBase
{
    /// <summary>
    /// 用于区分Buff，每个Buff Id都是独一无二的
    /// 因为我们不能，也不应该关心具体Buff的Id，所以这里直接自动生成
    /// </summary>
    [HideInInspector]
    [LabelText("Buff的Id")]
    [BoxGroup("必填项")]
    public long BuffId { get; set; }

    [LabelText("Buff的添加目标")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public BuffTargetTypes BuffTargetTypes { get; set; }

    [LabelText("Buff的基本特征")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public BuffBaseType BuffBaseType { get; set; }

    [LabelText("Buff类型")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public virtual BuffWorkType BuffWorkType { get; set; }

    [BoxGroup("选填项")]
    [LabelText("Buff是否状态栏可见")]
    [ShowInInspector]
    public bool Base_isVisualable { get; set; }

    [ShowIf("Base_isVisualable")]
    [LabelText("Buff图标的名称")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public string SpritePath { get; set; }

    [Tooltip("是否可以叠加(不能叠加就刷新，叠加满也刷新)")]
    [LabelText("是否叠加")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public bool CanOverlay { get; set; }

    [ShowIf("CanOverlay")]
    [LabelText("叠加层数")]
    [MinValue(1)]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public int TargetOverlay { get; set; } = 1;

    [ShowIf("CanOverlay")]
    [LabelText("最大叠加数")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public int MaxOverlay { get; set; }

    [LabelText("要抛出的事件ID，如果有的话")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public List<VTD_EventId> EventIds { get; set; } = new List<VTD_EventId>();

    [LabelText("Buff持续时间")]
    [Tooltip("-1代表永久,0代表只执行一次,单位ms")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public long SustainTime { get; set; }

    [LabelText("Buff基础数值影响者")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public BuffBaseDataEffectTypes BaseBuffBaseDataEffectTypes { get; set; }

    [LabelText("基础数值")]
    [BoxGroup("选填项")]
    [ShowInInspector]
    public int BasicValue { get; set; }

    [Tooltip("具体的加成(可能会一个效果多种加成方式)，例如法强加成")]
    [BoxGroup("选填项")]
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    [ShowInInspector]
    public Dictionary<BuffAdditionTypes, float> AdditionValue { get; set; } =
        new Dictionary<BuffAdditionTypes, float>();

    [LabelText("修改的属性")]
    [ShowIf("@BuffWorkType.Contains(BuffWorkType.ChangeProp)")]
    [BoxGroup("选填项")]
    [ValueDropdown("@NumericType.Str2TypeDoubleMap.Keys")]
    public string PropTypeStr { get; set; } = nameof(NumericType.None);

    public int PropType => NumericType.Str2TypeDoubleMap.GetValueByKey(PropTypeStr);

}