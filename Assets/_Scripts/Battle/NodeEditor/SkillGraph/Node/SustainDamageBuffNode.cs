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

    public override BuffNodeDataBase GetBuffNodeData()
    {
        return SkillBuffBases;
    }
}