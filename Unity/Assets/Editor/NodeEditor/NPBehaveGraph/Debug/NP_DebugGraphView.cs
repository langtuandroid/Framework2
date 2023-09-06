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

    public NP_BlackBoardDataManager NpBlackBoardDataManager;

    public NP_DebugGraphView(EditorWindow window) : base(window)
    {
        InitType();
    }

    private DoubleMap<Type, string> viewNode2BehaveName = new();
    private DoubleMap<Type, string> viewNode2BuffTypeName = new();
    private DoubleMap<Node, BaseNodeView> behaveNode2View = new();
    private NP_RuntimeTree currentTree;

    public void Refresh(NP_RuntimeTree tree)
    {
        behaveNode2View.Clear();
        RemoveGroups();
        RemoveNodeViews();
        RemoveEdges();
        CreateGraph(tree);
    }

    private void CreateGraph(NP_RuntimeTree tree)
    {
        currentTree = tree;
        Vector2 pos = Vector2.zero;
        BaseNodeView viewNode =
            AddNode(BaseNode.CreateFromType(viewNode2BehaveName.GetKeyByValue(tree.RootNode.Name), pos));
        (viewNode.nodeTarget as NP_NodeBase).Debug_SetNodeData(tree.RootNode.DebugData);
        behaveNode2View.Add(tree.RootNode, viewNode);
        GenAllChildren(viewNode, tree.RootNode, pos);
        GenAllBuff(tree.BelongNP_DataSupportor);
        GenBlackboard(tree.BelongNP_DataSupportor);
        EditorCoroutine.StartCoroutine(Update());
    }

    IEnumerator Update()
    {
        var wait = new WaitForSeconds(0.2f);
        yield return wait;
        AutoSortLayout();
        while (behaveNode2View != null)
        {
            yield return wait;
            behaveNode2View.ForEach((node, view) =>
            {
                if (node is NPBehave.Root)
                {
                    return;
                }

                view.SetNodeColor(node.IsActive ? Color.green : Color.white);
            });
            NpBlackBoardDataManager.RefreshFromDataSupporter(currentTree.BelongNP_DataSupportor);
        }
    }

    private void GenAllChildren(BaseNodeView parent, Node node, Vector2 pos)
    {
        if (node is Container container)
        {
            foreach (Node child in container.DebugChildren)
            {
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

    private void GenAllBuff(NP_DataSupportor dataSupportor)
    {
        Vector2 pos = new Vector2(0, 600);
        foreach (BuffNodeDataBase nodeDataBase in dataSupportor.BuffNodeDataDic.Values)
        {
            BaseNodeView viewNode = null;
            if (nodeDataBase is SkillDesNodeData)
            {
                viewNode = AddNode(BaseNode.CreateFromType(typeof(BuffDescriptionNode), pos));
            }
            else
            {
                Type viewNodeType =
                    viewNode2BuffTypeName.GetKeyByValue((nodeDataBase as NormalBuffNodeData).BuffData.GetType().Name);
                if (viewNodeType == null)
                {
                    Debug.LogWarning($"{nodeDataBase}找不到对应的view");
                    continue;
                }

                viewNode = AddNode(BaseNode.CreateFromType(viewNodeType, pos));
            }

            pos.x += 200;

            (viewNode.nodeTarget as BuffNodeBase).Debug_SetNodeData(nodeDataBase);
        }
    }

    private void GenBlackboard(NP_DataSupportor dataSupportor)
    {
        NpBlackBoardDataManager = new NP_BlackBoardDataManager();
        NpBlackBoardDataManager.RefreshFromDataSupporter(dataSupportor);
    }

    private void InitType()
    {
        if (viewNode2BehaveName.Keys.Count > 0)
        {
            return;
        }

        PropertyInfo nodeProp =
            typeof(NP_NodeBase).GetProperty("CreateNodeName", BindingFlags.Instance | BindingFlags.Public);
        PropertyInfo buffNodeProp =
            typeof(BuffNodeBase).GetProperty("CreateNodeName", BindingFlags.Instance | BindingFlags.Public);
        foreach (Type type in GetType().Assembly.GetTypes())
        {
            if (!type.IsAbstract && type.IsSubclassOf(typeof(NP_NodeBase)))
            {
                string name = nodeProp.GetValue(Activator.CreateInstance(type)) as string;
                viewNode2BehaveName.Add(type, name);
            }

            if (!type.IsAbstract && type.IsSubclassOf(typeof(BuffNodeBase)))
            {
                string name = buffNodeProp.GetValue(Activator.CreateInstance(type)) as string;
                viewNode2BuffTypeName.Add(type, name);
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