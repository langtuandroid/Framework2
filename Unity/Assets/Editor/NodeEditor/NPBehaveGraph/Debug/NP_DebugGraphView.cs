using System.Collections.Generic;
using System.Reflection;
using Framework;
using GraphProcessor;
using NPBehave;
using UnityEditor;
using UnityEngine;
using Root = NPBehave.Root;

public class NP_DebugGraphView : UniversalGraphView
{
    private NP_NodeView m_RootNodeView
    {
        get { return (NP_NodeView)nodeViews.Find(x => x.nodeTarget.name == "行为树根节点"); }
    }
    
    public NP_DebugGraphView(EditorWindow window) : base(window)
    {
    }

    public void Init(GameObject gameObject)
    {
        if (gameObject == null) return;
        var test = gameObject.GetComponent("Test");
        var root = test.GetType().GetField("root", BindingFlags.Instance | BindingFlags.Public).GetValue(test) as Root;
        var rootNode = AddNode(BaseNode.CreateFromType(typeof(NP_RootNode), Vector2.zero));
        Debug.Log(rootNode.outputPortViews[0].GetHashCode());
        var se = AddNode(BaseNode.CreateFromType(typeof(NP_SequenceNode), new Vector2(0.1f, 0.1f)));
        AddConnect(se.inputPortViews[0], rootNode.outputPortViews[0]);
        var log = AddNode(BaseNode.CreateFromType<NP_LogActionNode>(new Vector2(0.2f, 0.2f)));
        AddConnect(log.inputPortViews[0], se.outputPortViews[0]);
        var log1 = AddNode(BaseNode.CreateFromType<NP_LogActionNode>(new Vector2(0.2f, 0.2f)));
        AddConnect(log1.inputPortViews[0], se.outputPortViews[0]);
        var move = AddNode(BaseNode.CreateFromType<NP_MoveToTargetActionNode>(new Vector2(0.2f, 0.2f)));
        AddConnect(move.inputPortViews[0], se.outputPortViews[0]);
        var moveNode = move.nodeTarget as NP_MoveToTargetActionNode;
        moveNode.Debug_SetNodeData(root.MainNode.DebugData);
        // root.MainNode as NP_LogActionNode
        // logNode.NP_ActionNodeData
    }

    private void AddConnect(PortView input, PortView output)
    {
        var edgeView2 = CreateEdgeView();
        edgeView2.input = input;
        edgeView2.output = output;
        AddElement(edgeView2);
        ConnectView(edgeView2);
    }

    /// <summary>
    /// 自动排序布局
    /// </summary>
    public void AutoSortLayout()
    {
        var rootNodeView = m_RootNodeView;
        if (rootNodeView == null) return;

        // 先计算节点之间的联系（配置父节点和子节点）
        CalculateNodeRelationShip(rootNodeView);
        NodeAutoLayouter.Layout(new NPBehaveNodeConvertor().Init(rootNodeView));
    }

    private void CalculateNodeRelationShip(NP_NodeView rootNodeView)
    {
        rootNodeView.Parent = null;
        rootNodeView.Children.Clear();
        List<PortView> outputPort = rootNodeView.outputPortViews;
        List<PortView> inputPort = rootNodeView.inputPortViews;

        if (inputPort.Count > 0)
        {
            List<EdgeView> inputEdges = inputPort[0].GetEdges();
            if (inputEdges.Count > 0)
                rootNodeView.Parent = inputEdges[0].output.node as NP_NodeView;
            else
                Log.Error("当前行为树配置有误，请检查是否有节点未正确链接");
        }

        if (outputPort.Count > 0)
        {
            List<EdgeView> outputEdges = outputPort[0].GetEdges();
            if (outputEdges.Count > 0)
            {
                foreach (var outputEdge in outputEdges)
                {
                    var childNodeView = outputEdge.input.node as NP_NodeView;
                    CalculateNodeRelationShip(childNodeView);
                    rootNodeView.Children.Add(childNodeView);
                }

                // 根据x坐标进行排序
                rootNodeView.Children.Sort((x, y) => x.GetPosition().x.CompareTo(y.GetPosition().x));
            }
            else
            {
                Log.Error("当前行为树配置有误，请检查是否有节点未正确链接");
            }
        }
    }
}