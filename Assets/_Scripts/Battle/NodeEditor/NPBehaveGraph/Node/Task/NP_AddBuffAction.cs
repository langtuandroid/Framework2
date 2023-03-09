using System;

using Framework;
using Sirenix.OdinInspector;

[Title("给自己添加一个Buff", TitleAlignment = TitleAlignments.Centered)]
public class NP_AddBuffAction: NP_ClassForStoreAction
{
    public override Action GetActionToBeDone()
    {
        this.Action = this.AddBuff;
        return this.Action;
    }

    public void AddBuff()
    {
       Log.Msg("添加一个buff"); 
    }
}