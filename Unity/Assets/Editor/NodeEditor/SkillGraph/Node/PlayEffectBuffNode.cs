using GraphProcessor;

[NodeMenuItem("Buff/播放特效Buff", typeof (SkillGraph))]
public class PlayEffectBuffNode: BuffNodeBase
{
    public override string name => "播放特效Buff";

    public NormalBuffNodeData SkillBuffBases =
        new NormalBuffNodeData()
        {
            BuffDes = "播放特效Buff",
            BuffData = new PlayEffectBuffData() { }
        };

    public override BuffNodeDataBase GetBuffNodeData()
    {
        return SkillBuffBases;
    }
}