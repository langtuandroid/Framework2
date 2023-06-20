using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_RepeaterNodeData : NP_NodeDataBase
{
    [HideInEditorMode] public Repeater m_Repeater;

    [LabelText("重复次数,-1表示一直循环")]
    public int RepeatCount = -1;

    public override NodeType BelongNodeType => NodeType.Decorator;

    public override Node NP_GetNode()
    {
        return this.m_Repeater;
    }

    public override Decorator CreateDecoratorNode(Unit unit,NP_RuntimeTree runtimeTree, Node node)
    {
        this.m_Repeater = new Repeater(RepeatCount, node);
        return this.m_Repeater;
    }
}