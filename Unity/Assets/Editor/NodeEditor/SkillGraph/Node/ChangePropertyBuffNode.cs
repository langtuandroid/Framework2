using GraphProcessor;

[NodeMenuItem("Buff/修改属性Buff", typeof (SkillGraph))]
public class ChangePropertyBuffNode: BuffNodeBase
{
    public override string name => "修改属性Buff";

    public NormalBuffNodeData SkillBuffBases =
        new NormalBuffNodeData()
        {
            BuffDes = "修改属性Buff",
            BuffData = new ChangePropertyBuffData() { }
        };

    public override string CreateNodeName => nameof(ChangePropertyBuffData);

    public override BuffNodeDataBase GetBuffNodeData()
    {
        return SkillBuffBases;
    }

    public override void Debug_SetNodeData(object data)
    {
        this.SkillBuffBases = data as NormalBuffNodeData;
    }
}