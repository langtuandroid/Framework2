using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;

public class FlashDamageBuffData : BuffDataBase
{
    [LabelText("伤害类型")] [BoxGroup("自定义项")] [ShowInInspector] [BsonElement]
    public SkillDamageTypes DamageType;

    [BoxGroup("自定义项")] [LabelText("伤害附带的信息")]
    [BsonElement]
    public string CustomData;
}