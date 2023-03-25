using NPBehave;
using Sirenix.OdinInspector;

public class NP_ParallelNodeData : NP_NodeDataBase
{
    [HideInEditorMode] private Parallel m_ParallelNode;

    [LabelText("成功政策")] public Parallel.Policy SuccessPolicy = Parallel.Policy.ALL;

    [LabelText("失败政策")] public Parallel.Policy FailurePolicy = Parallel.Policy.ALL;

    public override Composite CreateComposite(Node[] nodes)
    {
        this.m_ParallelNode = new Parallel(SuccessPolicy, FailurePolicy, nodes);
        return this.m_ParallelNode;
    }

    public override NodeType BelongNodeType => NodeType.Composite;

    public override Node NP_GetNode()
    {
        return this.m_ParallelNode;
    }
}