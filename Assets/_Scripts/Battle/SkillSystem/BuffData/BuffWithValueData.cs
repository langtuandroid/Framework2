using System.Collections.Generic;
using Framework;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuffWithValueData : BuffDataBase
{
    
    [LabelText("伤害类型")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public SkillDamageTypes DamageType { get; set; }
    
    [LabelText("Buff基础数值影响者")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public BuffBaseDataEffectTypes BaseBuffBaseDataEffectTypes { get; set; }
    
    [LabelText("基础伤害")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public int BasicValue { get; set; }

    [Tooltip("具体的加成(可能会一个效果多种加成方式)，例如法强加成")]
    [BoxGroup("必填项")]
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    [ShowInInspector]
    public Dictionary<BuffAdditionTypes, float> AdditionValue { get; set; } =
        new Dictionary<BuffAdditionTypes, float>();

    [LabelText("Buff类型")]
    [BoxGroup("必填项")]
    [ShowInInspector]
    public BuffWorkTypes BuffWorkType { get; set; } = BuffWorkTypes.ChangeProperty;

    [LabelText("修改的属性")] [ShowInInspector]
    [BoxGroup("必填项")]
    [ValueDropdown("@NumericType.Str2TypeDoubleMap.Keys")]
    public string PropTypeStr { get; set; }

    public int PropType => NumericType.Str2TypeDoubleMap.GetValueByKey(PropTypeStr);
}