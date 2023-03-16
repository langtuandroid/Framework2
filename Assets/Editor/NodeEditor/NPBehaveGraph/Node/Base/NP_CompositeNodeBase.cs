using GraphProcessor;
using Plugins.NodeEditor;
using UnityEngine;

public abstract class NP_CompositeNodeBase : NP_NodeBase
{
    [Input("NPBehave_PreNode"), Vertical]
    [HideInInspector]
    public NP_NodeBase PrevNode;

    [Output("NPBehave_NextNode"), Vertical]
    [HideInInspector]
    public NP_NodeBase NextNode;
}