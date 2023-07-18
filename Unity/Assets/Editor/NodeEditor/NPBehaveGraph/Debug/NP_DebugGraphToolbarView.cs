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
            () => { (m_BaseGraphView as NP_DebugGraphView).Init(GameObject.Find("UnitRoot")); }, false);
    }
}