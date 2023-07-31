using System;

using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("打印信息", TitleAlignment = TitleAlignments.Centered)]
public class NP_LogAction : NP_ClassForStoreAction
{
    [LabelText("信息")] public IBlackboardOrValue LogInfo;

    public override Action GetActionToBeDone()
    {
        return TestLog;
    }

    private void TestLog()
    {
        Log.Msg(Time.time + "  " + LogInfo.GetObjValue(BelongtoRuntimeTree.GetBlackboard()));
    }
}