using Framework;
using NPBehave;
using Sirenix.OdinInspector;

[BoxGroup("等待结点数据")]
[HideLabel]
public class NP_WaitNodeData : NP_NodeDataBase
{
    [HideInEditorMode] private Wait m_WaitNode;

    public override NodeType BelongNodeType => NodeType.Task;

    [LabelText("等待时间的黑板值")]
    public NP_BlackBoardRelationData<float> NPBlackBoardRelationData = new ();

    [LabelText("等待时间")]
    public float Time;

    public override Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
    {
        if (string.IsNullOrEmpty(NPBlackBoardRelationData.BBKey))
        {
            this.m_WaitNode = new Wait(Time);
        }
        else
        {
            this.m_WaitNode = new Wait(this.NPBlackBoardRelationData.BBKey);
        }

        return this.m_WaitNode;
    }

    public override NPBehave.Node NP_GetNode()
    {
        return this.m_WaitNode;
    }
}