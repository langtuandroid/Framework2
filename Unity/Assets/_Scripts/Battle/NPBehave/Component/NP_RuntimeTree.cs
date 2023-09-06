using Framework;
using NPBehave;
using Root = NPBehave.Root;

public class NP_RuntimeTree : Entity, IAwakeSystem<NP_DataSupportor, NP_SyncComponent, Unit>
{
    /// <summary>
    /// NP行为树根结点
    /// </summary>
    private Root m_RootNode;

    /// <summary>
    /// 所归属的数据块
    /// </summary>
    public NP_DataSupportor BelongNP_DataSupportor;

    /// <summary>
    /// 所归属的Unit
    /// </summary>
    public Unit BelongToUnit;

    public NP_SyncComponent NpSyncComponent;

    public Clock GetClock()
    {
        return NpSyncComponent.SyncContext.GetClock();
    }

    /// <summary>
    /// 设置根结点
    /// </summary>
    /// <param name="rootNode"></param>
    public void SetRootNode(Root rootNode)
    {
        this.m_RootNode = rootNode;
        if (!m_RootNode.IsLoop)
        {
            m_RootNode.OnFinish += ()=>
            {
                (parent as NP_RuntimeTreeManager)?.RemoveTree(Id);
            };
        }
    }

    public string DebugName { get; private set; }

    public void SetDebugName(string debugName)
    {
        DebugName = debugName;
    }

    public Root RootNode => m_RootNode;

    /// <summary>
    /// 获取黑板
    /// </summary>
    /// <returns></returns>
    public Blackboard GetBlackboard()
    {
        if (m_RootNode == null)
        {
            Log.Error($"行为树的根节点为空");
        }

        if (m_RootNode.Blackboard == null)
        {
            Log.Error($"行为树的黑板实例为空");
        }

        return this.m_RootNode.Blackboard;
    }

    /// <summary>
    /// 开始运行行为树
    /// </summary>
    public void Start()
    {
        this.m_RootNode.Start();
    }

    /// <summary>
    /// 终止行为树
    /// </summary>
    public void Finish()
    {
        this.m_RootNode.CancelWithoutReturnResult();
        this.m_RootNode = null;
        this.BelongNP_DataSupportor = null;
    }

    public void Awake(NP_DataSupportor belongNP_DataSupportor, NP_SyncComponent npSyncComponent, Unit belongToUnit)
    {
        BelongToUnit = belongToUnit;
        BelongNP_DataSupportor = belongNP_DataSupportor;
        NpSyncComponent = npSyncComponent;
    }
}