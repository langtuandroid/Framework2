using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;
using MatchType = NPBehave.MatchType;

[BoxGroup("黑板多条件节点配置"), GUIColor(0.961f, 0.902f, 0.788f, 1f)]
[HideLabel]
public class NP_BlackboardMultipleConditionsNodeData : NP_NodeDataBase
{
    [HideInEditorMode] private BlackboardMultipleConditions m_BlackboardMultipleConditions;

    [LabelText("对比内容")] public List<MatchInfo> MatchInfos = new List<MatchInfo>();

    [LabelText("逻辑类型")] public MatchType MatchType;

    [LabelText("终止条件")] public Stops Stop = Stops.IMMEDIATE_RESTART;

    public override NodeType BelongNodeType => NodeType.Decorator;

    public override Decorator CreateDecoratorNode(NP_RuntimeTree runtimeTree, Node node)
    {
        this.m_BlackboardMultipleConditions =
            new BlackboardMultipleConditions(this.MatchInfos, this.MatchType, this.Stop, node);
        //此处的value参数可以随便设，因为我们在游戏中这个value是需要动态改变的
        return this.m_BlackboardMultipleConditions;
    }

    public override NPBehave.Node NP_GetNode()
    {
        return this.m_BlackboardMultipleConditions;
    }
}