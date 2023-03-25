using GraphProcessor;
using UnityEngine;

public class BuffNodeBase: BaseNode
{
    [Input("InputBuff", allowMultiple = true)]
    [HideInInspector]
    public BuffNodeBase PrevNode;
        
    [Output("OutputBuff", allowMultiple = true)]
    [HideInInspector]
    public BuffNodeBase NextNode;

    public override Color color => Color.green;

    public virtual void AutoAddLinkedBuffs()
    {
            
    }
        
    public virtual BuffNodeDataBase GetBuffNodeData()
    {
        return null;
    }
}