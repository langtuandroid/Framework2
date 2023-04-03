using System;

using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("打印信息", TitleAlignment = TitleAlignments.Centered)]
public class NP_LogAction : NP_ClassForStoreAction
{
    [LabelText("信息")] public string LogInfo;

    public override Action GetActionToBeDone()
    {
        this.Action = this.TestLog;
        return this.Action;
    }

    private void TestLog()
    {
        Log.Msg(Time.time + "  " + LogInfo);
    }
}