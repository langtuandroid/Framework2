using GraphProcessor;

[NodeMenuItem("Buff/瞬时伤害Buff", typeof (SkillGraph))]
public class FlashDamageBuffNode: BuffNodeBase
{
    public override string name => "瞬时伤害Buff";

    public NormalBuffNodeData SkillBuffBases =
        new NormalBuffNodeData()
        {
            BuffDes = "瞬时伤害Buff",
            BuffData = new FlashDamageBuffData() {}
        };

    public override string CreateNodeName => nameof(FlashDamageBuffData);

    public override BuffNodeDataBase GetBuffNodeData()
    {
        return SkillBuffBases;
    }

    public override void Debug_SetNodeData(object data)
    {
        SkillBuffBases = data as NormalBuffNodeData;
    }
}