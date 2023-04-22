using System.Collections.Generic;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public abstract class NP_NodeDataBase
{
    /// <summary>
    /// 此结点ID
    /// </summary>
    [LabelText("此结点ID")] [HideInEditorMode]
    public long id;

    public abstract NodeType BelongNodeType { get; }

    /// <summary>
    /// 与此结点相连的ID
    /// </summary>
    [HideInEditorMode] public List<long> LinkedIds = new List<long>();

    [BoxGroup("结点信息描述")] [HideLabel] public string NodeDes;

    /// <summary>
    /// 获取结点
    /// </summary>
    /// <returns></returns>
    public abstract Node NP_GetNode();

    /// <summary>
    /// 创建组合结点
    /// </summary>
    /// <returns></returns>
    public virtual Composite CreateComposite(Node[] nodes)
    {
        return null;
    }
    
    /// <summary>
    /// 创建装饰结点
    /// </summary>
    /// <param name="unitId">行为树归属的Unit</param>
    /// <param name="runtimeTree">运行时归属的行为树</param>
    /// <param name="node">所装饰的结点</param>
    /// <returns></returns>
    public virtual Decorator CreateDecoratorNode(Unit unit, NP_RuntimeTree runtimeTree, Node node)
    {
        return null;
    }

    /// <summary>
    /// 创建任务节点
    /// </summary>
    /// <param name="unitId">行为树归属的Unit</param>
    /// <param name="runtimeTree">运行时归属的行为树</param>
    /// <returns></returns>
    public virtual Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
    {
        return null;
    }

    public override string ToString()
    {
        return GetType().ToString();
    }
}

public enum NodeType
{
    Composite,
    Decorator,
    Task,
}