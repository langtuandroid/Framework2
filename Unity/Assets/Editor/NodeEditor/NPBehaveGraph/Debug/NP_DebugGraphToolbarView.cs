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

    private static NPBehaveToolbarView.BlackboardInspectorViewer _BlackboardInspectorViewer;

    public static NPBehaveToolbarView.BlackboardInspectorViewer BlackboardInspector
    {
        get
        {
            if (_BlackboardInspectorViewer == null)
            {
                _BlackboardInspectorViewer =
                    ScriptableObject.CreateInstance<NPBehaveToolbarView.BlackboardInspectorViewer>();
            }

            return _BlackboardInspectorViewer;
        }
    }

    protected override void AddButtons()
    {
        base.AddButtons();
        AddButton(new GUIContent("选择"),
            () => { (m_BaseGraphView as NP_DebugGraphView).Init(Selection.activeGameObject); }, false);
        AddButton(new GUIContent("自动布局"),
            () => { (m_BaseGraphView as NP_DebugGraphView).AutoSortLayout(); }, false);
        AddButton(new GUIContent("Blackboard", "打开Blackboard数据面板"),
            () =>
            {
                BlackboardInspector.NpBlackBoardDataManager =
                    (this.m_BaseGraphView as NP_DebugGraphView).NpBlackBoardDataManager;
                Selection.activeObject = BlackboardInspector;
            }, false);
    }
}