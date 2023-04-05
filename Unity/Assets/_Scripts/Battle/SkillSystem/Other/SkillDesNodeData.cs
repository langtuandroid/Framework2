using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

[BsonIgnoreExtraElements]
[GUIColor(0.4f, 0.8f, 1)]
public class SkillDesNodeData : BuffNodeDataBase
{
    [TabGroup("基础信息")] [LabelText("技能名称")] public string SkillName;

    [TabGroup("基础信息")] [HideLabel] [Title("技能描述", Bold = false)] [MultiLineProperty(10)]
    public string SkillDescribe;

    [TabGroup("基础信息")] [Title("技能资源名,第一个是图标")]
    public List<string> SkillABInfo = new();

    [TabGroup("基础信息")] [HideLabel] [Title("技能消耗类型")]
    public SkillCostTypes SkillCostTypes = SkillCostTypes.None;

    [TabGroup("基础信息")]
    [HideLabel]
    [Title("技能CD", Bold = false)]
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<int, long> SkillCD = new ();

    [TabGroup("基础信息")]
    [HideLabel]
    [Title("技能消耗", Bold = false)]
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<int, float> SkillCost = new();

    [TabGroup("基础信息")] [Title("技能类型", Bold = false)] [HideLabel]
    public SkillTypes SkillTypes = SkillTypes.NoBreak;

    [TabGroup("基础信息")] [Title("技能指示器类型", Bold = false)] [HideLabel] [HideIf("SkillTypes", SkillTypes.Passive)]
    public SkillReleaseMode SkillReleaseMode;

    [TabGroup("基础信息")] [Title("伤害类型", Bold = false)] [HideLabel]
    public SkillDamageTypes DamageType;
}