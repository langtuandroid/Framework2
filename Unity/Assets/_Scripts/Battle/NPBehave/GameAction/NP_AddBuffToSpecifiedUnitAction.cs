using System;
using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;

[Title("给指定Unit添加Buff，支持多个对象", TitleAlignment = TitleAlignments.Centered)]
public class NP_AddBuffToSpecifiedUnitAction : NP_ClassForStoreAction
{
    [LabelText("要添加的Buff的信息")] public VTD_BuffInfo BuffDataInfo = new VTD_BuffInfo();

    [LabelText("是否包含多个目标")] public bool IsMultiTarget = false;

    [LabelText("添加目标Id")] [ShowIf("IsMultiTarget")]
    public NP_BlackBoardRelationData<List<long>> Targets = new();

    [HideIf("IsMultiTarget")] [LabelText("添加目标Id")]
    public NP_BlackBoardRelationData<long> Target = new();

    [LabelText("是否包含自己")] public bool IncludeSelf = true;

    public override Action GetActionToBeDone()
    {
        this.Action = this.AddBuffToSpecifiedUnit;
        return this.Action;
    }

    private void AddBuffToSpecifiedUnit()
    {
        if (IncludeSelf)
        {
            BuffDataInfo.AutoAddBuff(BelongtoRuntimeTree.BelongNP_DataSupportor, BuffDataInfo.BuffNodeId.Value,
                BelongToUnit, BelongToUnit, BelongtoRuntimeTree);
        }

        UnitComponent unitComponent = BelongToUnit.DomainScene()
            .GetComponent<UnitComponent>();

        if (IsMultiTarget)
        {
            foreach (var targetUnitId in Targets.GetBlackBoardValue(
                         this.BelongtoRuntimeTree.GetBlackboard()))
            {
                BuffDataInfo.AutoAddBuff(BelongtoRuntimeTree.BelongNP_DataSupportor, BuffDataInfo.BuffNodeId.Value,
                    BelongToUnit, unitComponent.Get(targetUnitId), BelongtoRuntimeTree);
            }
        }
        else
        {
            BuffDataInfo.AutoAddBuff(BelongtoRuntimeTree.BelongNP_DataSupportor, BuffDataInfo.BuffNodeId.Value,
                BelongToUnit, unitComponent.Get(Target.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard())),
                BelongtoRuntimeTree);
        }
    }
}