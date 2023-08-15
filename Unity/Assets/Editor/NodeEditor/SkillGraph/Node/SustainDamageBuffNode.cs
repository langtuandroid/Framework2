using GraphProcessor;

[NodeMenuItem("Buff/持续伤害Buff", typeof (SkillGraph))]
public class SustainDamageBuffNode: BuffNodeBase
{
    public override string name => "持续伤害Buff";

    public NormalBuffNodeData SkillBuffBases =
        new NormalBuffNodeData()
        {
            BuffDes = "持续伤害Buff",
            BuffData = new SustainDamageBuffData() { }
        };

    public override string CreateNodeName => nameof(SustainDamageBuffData);

    public override BuffNodeDataBase GetBuffNodeData()
    {
        return SkillBuffBases;
    }

    public override void Debug_SetNodeData(object data)
    {
        SkillBuffBases = data as NormalBuffNodeData;
    }
}