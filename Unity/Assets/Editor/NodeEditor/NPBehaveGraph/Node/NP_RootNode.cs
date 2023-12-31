﻿using System;
using Framework;
using GraphProcessor;
using Plugins.NodeEditor;
using Sirenix.OdinInspector;
using UnityEngine;
using Root = NPBehave.Root;

[NodeMenuItem("NPBehave行为树/根结点", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/根结点", typeof(SkillGraph))]
public class NP_RootNode : NP_NodeBase
{
    public override Color color => Color.cyan;

    [LabelText("行为树根节点id")]
    [Sirenix.OdinInspector.ShowInInspector]
    public long Id
    {
        get => MRootNodeData.id;
        set
        {
        }
    }

    protected override void Enable()
    {
        base.Enable();
        if (MRootNodeData.id == 0)
        {
            MRootNodeData.id = IdGenerator.Instance.GenerateId();
        }
    }

    [Sirenix.OdinInspector.ShowInInspector]
    public override string name => "行为树根节点";

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

    public override string CreateNodeName => nameof(Root);

    public override void Debug_SetNodeData(object data)
    {
        MRootNodeData = (NP_RootNodeData)data;
    }
}