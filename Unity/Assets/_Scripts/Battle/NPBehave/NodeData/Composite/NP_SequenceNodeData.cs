using NPBehave;
using Sirenix.OdinInspector;

public class NP_SequenceNodeData : NP_NodeDataBase
{
    [HideInEditorMode] private Sequence m_SequenceNode;

    public override NodeType BelongNodeType => NodeType.Composite;

    public override Node NP_GetNode()
    {
        return this.m_SequenceNode;
    }

    public override Composite CreateComposite(Node[] nodes)
    {
        this.m_SequenceNode = new Sequence(nodes);
        return this.m_SequenceNode;
    }
}