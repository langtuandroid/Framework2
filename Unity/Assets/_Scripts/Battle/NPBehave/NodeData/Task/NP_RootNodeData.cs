using Framework;
using NPBehave;
using Sirenix.OdinInspector;
using Root = NPBehave.Root;

public class NP_RootNodeData : NP_NodeDataBase
{
    [HideInEditorMode] public Root m_Root;

    public override NodeType BelongNodeType => NodeType.Decorator;

    public override Decorator CreateDecoratorNode(Unit unit, NP_RuntimeTree runtimeTree, Node node)
    {
        this.m_Root = new Root(node, runtimeTree.GetClock());
        return this.m_Root;
    }

    public override Node NP_GetNode()
    {
        return this.m_Root;
    }
}