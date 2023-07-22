using Framework;
using UnityEngine;
using WaitUntil = NPBehave.WaitUntil;

public class NP_CalssForStoreWaitUntilAction
{

    /// <summary>
    /// 归属的UnitID
    /// </summary>
    [HideInInspector] public Unit BelongToUnit;

    /// <summary>
    /// 归属的运行时行为树实例
    /// </summary>
    [HideInInspector] public NP_RuntimeTree BelongtoRuntimeTree;

    /// <summary>
    /// 获取将要执行的委托函数，也可以在这里面做一些初始化操作
    /// </summary>
    /// <returns></returns>
    protected virtual void OnStart()
    {
    }

    protected virtual bool UntilFunc()
    {
        return true;
    }

    public WaitUntil _CreateNPBehaveAction(string debugName)
    {
        return new WaitUntil(OnStart, UntilFunc);
    }
}