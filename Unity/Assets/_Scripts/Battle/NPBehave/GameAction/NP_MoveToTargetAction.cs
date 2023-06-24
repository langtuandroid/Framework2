using System;
using Framework;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

[Title("移动到目标点", TitleAlignment = TitleAlignments.Centered)]
public class NP_MoveToTargetAction: NP_ClassForStoreAction
{
    
    [LabelText("目标")]
    public NP_BlackBoardRelationData<long> TargetInsId = new ();
    
    [LabelText("目标位置")]
    public NP_BlackBoardRelationData<Vector3> TargetPosId = new ();

    public override Action GetActionToBeDone()
    {
        this.Action = this.MoveToRandomPos;
        return this.Action;
    }
 
    private void MoveToRandomPos()
    {
        var speed = this.BelongToUnit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
        Vector3 pos;
        if (!string.IsNullOrEmpty(TargetInsId.BBKey))
        {
            pos = BelongToUnit.Domain.GetComponent<UnitComponent>().Get(TargetInsId.GetTheBBDataValue())
                .Position;
            this.BelongToUnit.GetComponent<MoveComponent>().MoveTo(pos, speed);
            TargetPosId.SetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard(), pos);
        }
        else
        {
            pos = TargetPosId.GetTheBBDataValue();
        }

        this.BelongToUnit.GetComponent<MoveComponent>().MoveTo(pos, speed);
    }
}