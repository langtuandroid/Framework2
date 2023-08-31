using GraphProcessor;

[NodeMenuItem("Buff/治疗Buff", typeof (SkillGraph))]
public class TreatmentBuffNode: BuffNodeBase
{
    public override string name => "治疗Buff";

    public NormalBuffNodeData SkillBuffBases =
        new NormalBuffNodeData()
        {
            BuffDes = "治疗Buff",
            BuffData = new TreatmentBuffData() { }
        };

    public override string CreateNodeName => nameof(TreatmentBuffData);

    public override BuffNodeDataBase GetBuffNodeData()
    {
        return SkillBuffBases;
    }

    public override void Debug_SetNodeData(object data)
    {
        this.SkillBuffBases = data as NormalBuffNodeData;
    }
}