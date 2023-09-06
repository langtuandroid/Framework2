using GraphProcessor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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
        AddToggle("行为树", true, v => graphView.ToggleView<NP_BehaveTreesView>());
        graphView.ToggleView<NP_BehaveTreesView>();
        AddButton(new GUIContent("选择"),
            () =>
            {
                graphView.GetPinned<NP_BehaveTreesView>().Refresh(Selection.activeGameObject,
                    m_BaseGraphView as NP_DebugGraphView);
            }, false);
        AddButton(new GUIContent("Blackboard", "打开Blackboard数据面板"),
            () =>
            {
                BlackboardInspector.NpBlackBoardDataManager =
                    (this.m_BaseGraphView as NP_DebugGraphView).NpBlackBoardDataManager;
                Selection.activeObject = BlackboardInspector;
            }, false);
    }
}