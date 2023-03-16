using GraphProcessor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class UniversalGraphWindow : BaseGraphWindow
{
    private bool m_HasInitGUIStyles;
    protected MiniMap m_MiniMap;
    protected UniversalToolbarView m_ToolbarView;

    protected override void OnEnable()
    {
        base.OnEnable();

        titleContent = new GUIContent("Universal Graph",
            AssetDatabase.LoadAssetAtPath<Texture2D>(
                $"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/Icon_Dark.png"));
        m_HasInitGUIStyles = false;
    }

    private void OnGUI()
    {
        InitGUIStyles(ref m_HasInitGUIStyles);

        m_MiniMap?.SetPosition(new Rect(position.size.x - 205, position.size.y - 205, 200, 200));
    }

    protected override void InitializeWindow(BaseGraph graph)
    {
        graphView = new UniversalGraphView(this);

        m_MiniMap = new MiniMap { anchored = true };
        graphView.Add(m_MiniMap);

        m_ToolbarView = new UniversalToolbarView(graphView, m_MiniMap, graph);
        graphView.Add(m_ToolbarView);
    }

    /// <summary>
    ///     初始化绘制此GraphView需要用到的GUIStyle
    /// </summary>
    private void InitGUIStyles(ref bool result)
    {
        if (!result)
        {
            EditorGUIStyleHelper.SetGUIStylePadding(nameof(EditorStyles.toolbarButton), new RectOffset(15, 15, 0, 0));
            result = true;
        }
    }

    protected override void InitializeGraphView(BaseGraphView view)
    {
        // graphView.OpenPinned< ExposedParameterView >();
        // toolbarView.UpdateButtonStatus();
    }
}