using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("移动到目标点", TitleAlignment = TitleAlignments.Centered)]
public class NP_MoveToTargetAction: NP_ClassForStoreAction
{
    
    [LabelText("目标")]
    public NP_BlackBoardRelationData<long> TargetInsId = new ();

    public override Action GetActionToBeDone()
    {
        this.Action = this.MoveToRandomPos;
        return this.Action;
    }

    private void MoveToRandomPos()
    {
        BelongToUnit.GetComponent<FollowTargetComponent>().Follow(TargetInsId.GetTheBBDataValue(), 1);
    }
}