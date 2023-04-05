using System.Collections.Generic;
using UnityEngine;

public class NPBehaveNodeConvertor : INodeForLayoutConvertor
{
    public float SiblingDistance => 50;

    public object PrimRootNode => m_PrimRootNode;
    private object m_PrimRootNode;

    private NodeAutoLayouter.TreeNode m_LayoutRootNode;
    public NodeAutoLayouter.TreeNode LayoutRootNode => m_LayoutRootNode;

    public INodeForLayoutConvertor Init(object primRootNode)
    {
        m_PrimRootNode = primRootNode;
        return this;
    }

    public NodeAutoLayouter.TreeNode PrimNode2LayoutNode()
    {
        NP_NodeView graphNodeViewBase = m_PrimRootNode as NP_NodeView;

        m_LayoutRootNode =
            new NodeAutoLayouter.TreeNode(graphNodeViewBase.layout.size.x + SiblingDistance,
                graphNodeViewBase.layout.size.y,
                graphNodeViewBase.layout.position.y,
                NodeAutoLayouter.CalculateMode.Vertical |
                NodeAutoLayouter.CalculateMode.Positive);
        Convert2LayoutNode(graphNodeViewBase,
            m_LayoutRootNode, graphNodeViewBase.layout.position.y + graphNodeViewBase.layout.size.y,
            NodeAutoLayouter.CalculateMode.Vertical |
            NodeAutoLayouter.CalculateMode.Positive);
        return m_LayoutRootNode;
    }

    /// <summary>
    /// 上个节点的左下角坐标点.y
    /// </summary>
    /// <param name="rootPrimNode"></param>
    /// <param name="rootLayoutNode"></param>
    /// <param name="lastHeightPoint"></param>
    /// <param name="calculateMode"></param>
    private void Convert2LayoutNode(NP_NodeView rootPrimNode,
        NodeAutoLayouter.TreeNode rootLayoutNode, float lastHeightPoint,
        NodeAutoLayouter.CalculateMode calculateMode)
    {
        if (rootPrimNode.Children != null)
        {
            foreach (var childNode in rootPrimNode.Children)
            {
                NodeAutoLayouter.TreeNode childLayoutNode =
                    new NodeAutoLayouter.TreeNode(childNode.layout.size.x + SiblingDistance,
                        childNode.layout.size.y,
                        lastHeightPoint + SiblingDistance, calculateMode);
                rootLayoutNode.AddChild(childLayoutNode);
                Convert2LayoutNode(childNode, childLayoutNode,
                    lastHeightPoint + SiblingDistance + childNode.layout.size.y,
                    calculateMode);
            }
        }
    }

    public void LayoutNode2PrimNode()
    {
        Vector2 calculateRootResult = m_LayoutRootNode.GetPos();

        NP_NodeView root = m_PrimRootNode as NP_NodeView;
        root.SetPosition(new Rect(calculateRootResult, Vector2.zero));

        Convert2PrimNode(m_PrimRootNode as NP_NodeView, m_LayoutRootNode);
    }

    private void Convert2PrimNode(NP_NodeView rootPrimNode,
        NodeAutoLayouter.TreeNode rootLayoutNode)
    {
        if (rootPrimNode.Children != null)
        {
            List<NP_NodeView> children = rootPrimNode.Children;
            for (int i = 0; i < rootLayoutNode.children.Count; i++)
            {
                Vector2 calculateResult = rootLayoutNode.children[i].GetPos();

                children[i].SetPosition(new Rect(calculateResult, Vector2.zero));

                Convert2PrimNode(children[i], rootLayoutNode.children[i]);
            }
        }
    }
}