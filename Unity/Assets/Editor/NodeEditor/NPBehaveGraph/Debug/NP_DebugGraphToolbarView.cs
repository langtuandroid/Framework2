using GraphProcessor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NP_DebugGraphToolbarView : UniversalToolbarView
{
    public NP_DebugGraphToolbarView(BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph) : base(graphView,
        miniMap, baseGraph)
    {
    }

    protected override void AddButtons()
    {
        base.AddButtons();
        AddButton(new GUIContent("选择"),
            () => { (m_BaseGraphView as NP_DebugGraphView).Init(Selection.activeGameObject); }, false);
        AddButton(new GUIContent("自动布局"),
            () => { (m_BaseGraphView as NP_DebugGraphView).AutoSortLayout(); }, false);
        AddButton(new GUIContent("刷新"),
            () => { (m_BaseGraphView as NP_DebugGraphView).Update(); }, false);
    }
}