using Framework;
using Sirenix.OdinInspector;

[Title("销毁自己和相关联的Collider", TitleAlignment = TitleAlignments.Centered)]
public class NP_DestroySelfAndRelativeCollidersAction : NP_ClassForStoreAction
{
    public override System.Action GetActionToBeDone()
    {
        this.Action = this.DestroySelfAndRelativeCollider;
        return this.Action;
    }

    public void DestroySelfAndRelativeCollider()
    {
        Log.Msg("销毁碰撞体");
        UnitComponent unitComponent = BelongToUnit.DomainScene()
            .GetComponent<UnitComponent>();
        unitComponent.Remove(this.BelongtoRuntimeTree.BelongToUnit.Id);
    }
}