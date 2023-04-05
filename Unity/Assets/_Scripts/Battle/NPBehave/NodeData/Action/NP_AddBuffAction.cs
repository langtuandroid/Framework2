using System;

using Framework;
using Sirenix.OdinInspector;

[Title("给自己添加一个Buff", TitleAlignment = TitleAlignments.Centered)]
public class NP_AddBuffAction: NP_ClassForStoreAction
{
    [LabelText("要添加的Buff的信息")]
    public VTD_BuffInfo BuffDataInfo = new VTD_BuffInfo();

    public override Action GetActionToBeDone()
    {
        this.Action = this.AddBuff;
        return this.Action;
    }

    public void AddBuff()
    {
        BuffDataInfo.AutoAddBuff(BelongtoRuntimeTree.BelongNP_DataSupportor, this.BuffDataInfo.BuffNodeId.Value, BelongToUnit, BelongToUnit,
            BelongtoRuntimeTree);
    }
}