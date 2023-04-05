public interface INodeForLayoutConvertor
{
    /// <summary>
    /// 节点间的距离
    /// </summary>
    float SiblingDistance { get; }

    object PrimRootNode { get; }
    NodeAutoLayouter.TreeNode LayoutRootNode { get; }

    INodeForLayoutConvertor Init(object primRootNode);
    NodeAutoLayouter.TreeNode PrimNode2LayoutNode();
    void LayoutNode2PrimNode();
}