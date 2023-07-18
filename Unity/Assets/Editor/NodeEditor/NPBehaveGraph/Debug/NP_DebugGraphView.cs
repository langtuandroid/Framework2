using System.Reflection;
using GraphProcessor;
using NPBehave;
using UnityEditor;
using UnityEngine;

public class NP_DebugGraphView : UniversalGraphView
{
    public NP_DebugGraphView(EditorWindow window) : base(window)
    {
    }

    public void Init(GameObject gameObject)
    {
        if (gameObject == null) return;
        var test = gameObject.GetComponent("Test");
        var root = test.GetType().GetField("root", BindingFlags.Instance | BindingFlags.Public).GetValue(test) as Root;
        var rootNode = AddNode(BaseNode.CreateFromType(typeof(NP_RootNode), Vector2.zero));
        var se = AddNode(BaseNode.CreateFromType(typeof(NP_SequenceNode), Vector2.zero));
    }
}