using System;
using System.Collections.Generic;
using System.Linq;

using GraphProcessor;
using Plugins.NodeEditor;
using UnityEngine;
using UnityEngine.UIElements;

[NodeCustomEditor(typeof(NP_NodeBase))]
public class NP_NodeView : BaseNodeView
{
    [HideInInspector] public NP_NodeBase NpNodeBase;

    [HideInInspector] public NP_NodeView Parent;

    [HideInInspector] public new List<NP_NodeView> Children = new List<NP_NodeView>();

    public int ChildCount
    {
        get
        {
            if (this.Children == null)
            {
                return 0;
            }

            return this.Children.Count;
        }
    }

    public override void Enable()
    {
        NpNodeBase = this.nodeTarget as NP_NodeBase;
        NP_NodeDataBase nodeDataBase = (this.nodeTarget as NP_NodeBase).NP_GetNodeData();
        var textField = new TextField() { value = nodeDataBase.NodeDes ?? string.Empty };
        textField.style.marginTop = 4;
        textField.style.marginBottom = 4;
        textField.RegisterValueChangedCallback((changedDes) => { nodeDataBase.NodeDes = changedDes.newValue; });
        controlsContainer.Add(textField);
    }
}