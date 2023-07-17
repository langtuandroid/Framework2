using GraphProcessor;
using UnityEditor;

public class NP_DebugWindow : UniversalGraphWindow
{
    [MenuItem("Tools/Debug")]
    private static void OpenWindow()
    {
        var graph = CreateInstance<NP_DebugGraph>();
        NodeGraphWindowHelper.GetAndShowNodeGraphWindow<NP_DebugWindow>(graph)
            .InitializeGraph(graph);
    }

    protected override void InitializeWindow(BaseGraph graph)
    {
        graphView = new NP_DebugGraphView(this);
        m_ToolbarView = new NP_DebugGraphToolbarView(graphView, m_MiniMap, graph);
        graphView.Add(m_ToolbarView);
    }
}