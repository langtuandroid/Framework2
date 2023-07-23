using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework;
using Framework.Editor;
using GraphProcessor;
using NPBehave;
using Plugins.NodeEditor;
using UnityEditor;
using UnityEngine;
using Root = Framework.Root;

public class NP_DebugGraphView : UniversalGraphView
{
    private NP_NodeView m_RootNodeView
    {
        get { return (NP_NodeView)nodeViews.Find(x => x.nodeTarget.name == "行为树根节点"); }
    }
    
    public NP_DebugGraphView(EditorWindow window) : base(window)
    {
    }

    private DoubleMap<Type, string> viewNode2BehaveName = new();
    private DoubleMap<Node, BaseNodeView> behaveNode2View = new();
    
    public void Init(GameObject gameObject)
    {
        if (gameObject == null) return;
        if (gameObject.GetComponent<GoConnectedUnitId>() == null)
        {
            return;
        }

        behaveNode2View.Clear();

        InitType();
        long unitId = gameObject.GetComponent<GoConnectedUnitId>().UnitId;
        Unit unit = Root.Instance.Scene.GetComponent<CurrentScenesComponent>().Scene.GetComponent<UnitComponent>()
            .Get(unitId);
        Dictionary<long, NP_RuntimeTree> trees = unit.GetComponent<NP_RuntimeTreeManager>().runtimeId2Tree;
        KeyValuePair<long, NP_RuntimeTree> tree = trees.First();
        Vector2 pos = Vector2.zero;
        BaseNodeView viewNode =
            AddNode(BaseNode.CreateFromType(viewNode2BehaveName.GetKeyByValue(tree.Value.RootNode.Name), pos));
        (viewNode.nodeTarget as NP_NodeBase).Debug_SetNodeData(tree.Value.RootNode.DebugData);
        behaveNode2View.Add(tree.Value.RootNode, viewNode);
        GenAllChildren(viewNode, tree.Value.RootNode, pos);
        Debug.Log("创建完成");
        // AutoSortLayout();
        Debug.Log("布局完成");
    }

    public void Update()
    {
        behaveNode2View.ForEach((node, view) =>
        {
            if (node is NPBehave.Root)
            {
                return;
            }

            view.SetNodeColor(node.IsActive ? Color.green : Color.white);
        });
    }


    private void GenAllChildren(BaseNodeView parent, Node node, Vector2 pos)
    {
        if (node is Container container)
        {
            foreach (Node child in container.DebugChildren)
            {
                Debug.Log("创建" + child);
                pos += new Vector2(1f, 1f);
                Type viewNodeType = viewNode2BehaveName.GetKeyByValue(child.Name);
                if (viewNodeType == null)
                {
                    viewNodeType = typeof(NP_LogActionNode);
                    Debug.LogWarning($"{child}找不到对应的view");
                }

                BaseNodeView viewNode =
                    AddNode(BaseNode.CreateFromType(viewNodeType, pos));
                (viewNode.nodeTarget as NP_NodeBase).Debug_SetNodeData(child.DebugData);
                AddConnect(viewNode.inputPortViews[0], parent.outputPortViews[0]);
                behaveNode2View.Add(child, viewNode);
                GenAllChildren(viewNode, child, pos);
            }
        }
    }

    private void InitType()
    {
        if (viewNode2BehaveName.Keys.Count > 0)
        {
            return;
        }

        PropertyInfo nodeProp =
            typeof(NP_NodeBase).GetProperty("CreateNodeName", BindingFlags.Instance | BindingFlags.Public);
        foreach (Type type in GetType().Assembly.GetTypes())
        {
            if (!type.IsAbstract && type.IsSubclassOf(typeof(NP_NodeBase)))
            {
                string name = nodeProp.GetValue(Activator.CreateInstance(type)) as string;
                viewNode2BehaveName.Add(type, name);
            }
        }
    }
    
    private void AddConnect(PortView input, PortView output)
    {
        Debug.Log($"连接了 {input.owner.nodeTarget.GetType()} 和 {output.owner.nodeTarget.GetType()}");
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
        // Debug.Log(rootNodeView.nodeTarget);
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