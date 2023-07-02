using System;
using Framework;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

[Title("移动到目标点", TitleAlignment = TitleAlignments.Centered)]
public class NP_MoveToTargetAction: NP_ClassForStoreAction
{
    
    [LabelText("目标id")]
    public NP_BlackBoardRelationData<long> TargetInsId = new ();
    
    [LabelText("目标位置")]
    [Tooltip("如果填入目标id,会把目标位置设置成目标id的位置")]
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
            pos = BelongToUnit.Domain.GetComponent<UnitComponent>().Get(TargetInsId.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard()))
                .Position;
            this.BelongToUnit.GetComponent<MoveComponent>().MoveTo(pos, speed);
            TargetPosId.SetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard(), pos);
        }
        else
        {
            pos = TargetPosId.GetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard());
        }

        this.BelongToUnit.GetComponent<MoveComponent>().MoveTo(pos, speed);
    }
}