using GraphProcessor;
using Plugins.NodeEditor;
using Sirenix.OdinInspector;
using UnityEngine;

[NodeMenuItem("NPBehave行为树/根结点", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/根结点", typeof(SkillGraph))]
public class NP_RootNode : NP_NodeBase
{
    public override Color color => Color.cyan;

    public override string name { get; } = "行为树根节点";

    [Output("下个节点"), Vertical, HideInInspector]
    public BaseNode NextNode;

    [BoxGroup("根结点数据")]
    [HideReferenceObjectPicker]
    [HideLabel]
    public NP_RootNodeData MRootNodeData = new NP_RootNodeData { };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return this.MRootNodeData;
    }
}