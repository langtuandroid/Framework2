using NPBehave;
using Sirenix.OdinInspector;

public class NP_RepeaterNodeData : NP_NodeDataBase
{
    [HideInEditorMode] public Repeater m_Repeater;

    public override NodeType BelongNodeType => NodeType.Decorator;

    public override Node NP_GetNode()
    {
        return this.m_Repeater;
    }

    public override Decorator CreateDecoratorNode(NP_RuntimeTree runtimeTree, Node node)
    {
        this.m_Repeater = new Repeater(node);
        return this.m_Repeater;
    }
}