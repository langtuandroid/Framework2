﻿using System;
using GraphProcessor;
using NPBehave;

[NodeMenuItem("NPBehave行为树/Decorator/黑板条件节点", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Decorator/黑板条件节点", typeof (SkillGraph))]
public class NP_BlackboardConditionNode: NP_DecoratorNodeBase
{
    public override string name => "黑板条件结点";

    public NP_BlackboardConditionNodeData NP_BlackboardConditionNodeData =
        new NP_BlackboardConditionNodeData { NodeDes = "黑板条件结点" };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_BlackboardConditionNodeData;
    }

    public override string CreateNodeName => nameof(BlackboardCondition);

    public override void Debug_SetNodeData(object data)
    {
        NP_BlackboardConditionNodeData = (NP_BlackboardConditionNodeData)data;
    }
}