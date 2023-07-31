using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

/// <summary>
/// 用于封装一系列ActionNode，旨在增强易读性，默认只能是Action，而不能是Func
/// </summary>
[Title("宏行为节点", TitleAlignment = TitleAlignments.Centered)]
public class NP_MacroAction : NP_ClassForStoreAction
{
    [LabelText("宏行为节点集合")]
    public List<NP_ClassForStoreAction> NpClassForStoreActions = new List<NP_ClassForStoreAction>();

    public override Action GetActionToBeDone()
    {
        foreach (var npClassForStoreAction in NpClassForStoreActions)
        {
            npClassForStoreAction.BelongToUnit = this.BelongToUnit;
            npClassForStoreAction.BelongtoRuntimeTree = this.BelongtoRuntimeTree;
        }

        return DoMacro;
    }

    private void DoMacro()
    {
        //Log.Info("准备执行初始化的行为操作");
        foreach (var classForStoreAction in NpClassForStoreActions)
        {
            classForStoreAction.GetActionToBeDone().Invoke();
        }
    }
}