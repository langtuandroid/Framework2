using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_PlayAnimNodeData : NP_NodeDataBase
{
    private Sequence sequence;
    [LabelText("动画名字")]
    public BlackboardOrValue_String AnimName = new BlackboardOrValue_String("播放动画的名称");
    [LabelText("完成帧率")]
    public int FinishFrame;

    public override Node CreateCombineNode(Unit unit, NP_RuntimeTree runtimeTree)
    {
        sequence = new Sequence(new NP_ClassForStoreAction()
            {
                BelongtoRuntimeTree = runtimeTree, BelongToUnit = unit,
                Action = () =>
                    unit.GetComponent<PlayAnimComponent>().PlayAnim(AnimName.GetValue(runtimeTree.GetBlackboard()))
            }._CreateNPBehaveAction(),
            new WaitUntil(null,
                () => unit.GetComponent<PlayAnimComponent>()
                    .IsArriveTargetFrame(AnimName.GetValue(runtimeTree.GetBlackboard()), FinishFrame)));
        return this.sequence;
    }

    public override NodeType BelongNodeType => NodeType.CombineNode;

    public override Node NP_GetNode()
    {
        return this.sequence;
    }
}