using System;
using Framework;
using Sirenix.OdinInspector;

[Title("播放动画", TitleAlignment = TitleAlignments.Centered)]
public class NP_PlayAnimAction : NP_ClassForStoreAction
{
    public BlackboardOrValue_String AnimName = new BlackboardOrValue_String("播放动画的名称");
    
    public override Action GetActionToBeDone()
    {
        this.Action = this.PlayAnim;
        return this.Action;
    }

    private void PlayAnim()
    {
//        BelongToUnit.GetComponent<PlayAnimComponent>().PlayAnim(AnimName.GetValue(BelongtoRuntimeTree.GetBlackboard()));
    }
}