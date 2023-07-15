using Framework;
using NPBehave;
using Sirenix.OdinInspector;

[BoxGroup("等待结点数据")]
[HideLabel]
public class NP_WaitNodeData : NP_NodeDataBase
{
    [HideInEditorMode] private Wait m_WaitNode;

    public override NodeType BelongNodeType => NodeType.Task;

    [LabelText("等待时间")]
    public BlackboardOrValue_Float Time = new BlackboardOrValue_Float();

    public override Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
    {
        if (Time.UseBlackboard)
        {
            m_WaitNode = new Wait(Time.BlackboardKey.BBKey);
        }
        else
        {
            m_WaitNode = new Wait(Time.OriginValue);
        }
        return this.m_WaitNode;
    }

    public override NPBehave.Node NP_GetNode()
    {
        return this.m_WaitNode;
    }
}