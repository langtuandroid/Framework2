using GraphProcessor;
using UnityEngine;

public abstract class BuffNodeBase : BaseNode
{
    [Input("InputBuff", allowMultiple = true)]
    [HideInInspector]
    public BuffNodeBase PrevNode;
        
    [Output("OutputBuff", allowMultiple = true)]
    [HideInInspector]
    public BuffNodeBase NextNode;

    public override Color color => Color.green;

    public abstract string CreateNodeName { get; }

    public virtual void AutoAddLinkedBuffs()
    {
            
    }
        
    public virtual BuffNodeDataBase GetBuffNodeData()
    {
        return null;
    }

    public abstract void Debug_SetNodeData(object data);
}