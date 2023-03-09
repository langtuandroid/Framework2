using NPBehave;
using Sirenix.OdinInspector;

[BoxGroup("等待结点数据")]
[HideLabel]
public class NP_WaitNodeData : NP_NodeDataBase
{
    [HideInEditorMode] private Wait m_WaitNode;

    public override NodeType BelongNodeType => NodeType.Task;

    public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData();

    public override Task CreateTask(NP_RuntimeTree runtimeTree)
    {
        this.m_WaitNode = new Wait(this.NPBalckBoardRelationData.BBKey);
        return this.m_WaitNode;
    }

    public override NPBehave.Node NP_GetNode()
    {
        return this.m_WaitNode;
    }
}