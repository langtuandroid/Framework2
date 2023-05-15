using System;
using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;

[Title("给指定Unit添加Buff，支持多个对象", TitleAlignment = TitleAlignments.Centered)]
public class NP_AddBuffToSpecifiedUnitAction : NP_ClassForStoreAction
{
    [LabelText("要添加的Buff的信息")] public VTD_BuffInfo BuffDataInfo = new VTD_BuffInfo();

    [LabelText("添加目标Id")] public NP_BlackBoardRelationData<long> NPBalckBoardRelationData = new ();

    public override Action GetActionToBeDone()
    {
        this.Action = this.AddBuffToSpecifiedUnit;
        return this.Action;
    }

    public void AddBuffToSpecifiedUnit()
    {
        UnitComponent unitComponent = BelongToUnit.DomainScene()
            .GetComponent<UnitComponent>();

        foreach (var targetUnitId in NPBalckBoardRelationData.GetBlackBoardValue<List<long>>(
                     this.BelongtoRuntimeTree.GetBlackboard()))
        {
            BuffDataInfo.AutoAddBuff(BelongtoRuntimeTree.BelongNP_DataSupportor, BuffDataInfo.BuffNodeId.Value,
                BelongToUnit, unitComponent.Get(targetUnitId), BelongtoRuntimeTree);
        }
    }
}