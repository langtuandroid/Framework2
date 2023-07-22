using GraphProcessor;
using Sirenix.OdinInspector;

[NodeMenuItem("NPBehave行为树/Decorator/Repeater", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Decorator/Repeater", typeof(SkillGraph))]
public class NP_RepeaterNode : NP_DecoratorNodeBase
{
    public override string name => "重复执行结点";

    [BoxGroup("重复执行结点数据")] [HideReferenceObjectPicker] [HideLabel]
    public NP_RepeaterNodeData NpRepeaterNodeData = new NP_RepeaterNodeData { NodeDes = "重复执行结点数据" };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpRepeaterNodeData;
    }

    public override void Debug_SetNodeData(object data)
    {
        NpRepeaterNodeData = (NP_RepeaterNodeData)data;
    }
}