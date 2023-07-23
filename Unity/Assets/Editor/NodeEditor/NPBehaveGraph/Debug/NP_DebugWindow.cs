using System;
using Framework;
using Framework.Editor;
using GraphProcessor;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NP_DebugWindow : UniversalGraphWindow
{
    [MenuItem("Tools/Debug")]
    private static void OpenWindow()
    {
        var graph = CreateInstance<NP_DebugGraph>();
        CreateWindow<NP_DebugWindow>().InitializeGraph(graph);
    }

    protected override void InitializeWindow(BaseGraph graph)
    {
        graphView = new NP_DebugGraphView(this);
        m_ToolbarView = new NP_DebugGraphToolbarView(graphView, m_MiniMap, graph);
        graphView.Add(m_ToolbarView);
    }
}