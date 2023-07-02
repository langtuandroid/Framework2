using System;
using Sirenix.OdinInspector;

[Title("寻找目标", TitleAlignment = TitleAlignments.Centered)]
public class NP_FindTargetAction : NP_ClassForStoreAction
{
    [LabelText("目标")]
    public NP_BlackBoardRelationData<long> TargetInsId = new ();
    [LabelText("阵营")]
    public RoleCast RoleCast;
    [LabelText("标签")]
    public RoleTag RoleTag;
    
    public override Action GetActionToBeDone()
    {
        this.Action = this.FindTarget;
        return this.Action;
    }

    private void FindTarget()
    {
        BelongToUnit.GetComponent<FindTargetComponent>().FindTarget((id) =>
        {
            TargetInsId.SetBlackBoardValue(BelongtoRuntimeTree.GetBlackboard(), id);
        }, RoleCast, RoleTag);
    }
}