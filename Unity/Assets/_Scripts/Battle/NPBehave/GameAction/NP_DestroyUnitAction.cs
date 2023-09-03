using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("销毁Unit", TitleAlignment = TitleAlignments.Centered)]
public class NP_DestroyUnitAction : NP_ClassForStoreAction
{
    [LabelText("是否销毁关联的GameObject")]
    public BlackboardOrValue_Bool IsDestroyRelativeGameObject = new BlackboardOrValue_Bool(true);
    [LabelText("是否销毁自己")]
    public BlackboardOrValue_Bool IsDestroySelf = new BlackboardOrValue_Bool(true);
    [LabelText("销毁的目标")]
    [HideIf("@IsDestroySelf.GetEditorValue()")]
    public BlackboardOrValue_Long DestroyTarget = new BlackboardOrValue_Long();
    public override System.Action GetActionToBeDone()
    {
        return DestroySelfAndRelativeCollider;
    }

    public void DestroySelfAndRelativeCollider()
    {
        UnitComponent unitComponent = BelongToUnit.DomainScene()
            .GetComponent<UnitComponent>();
        long destroyUnitId = IsDestroySelf.GetValue(BelongtoRuntimeTree.GetBlackboard())
            ? BelongtoRuntimeTree.BelongToUnit.Id
            : DestroyTarget.GetValue(BelongtoRuntimeTree.GetBlackboard());
        Log.Msg("tree:", BelongtoRuntimeTree.Id, "销毁了", destroyUnitId);
        if (IsDestroyRelativeGameObject.GetValue(BelongtoRuntimeTree.GetBlackboard()))
        {
            unitComponent.Get(destroyUnitId).GetComponent<GameObjectComponent>()?.DestroyGameObject();
        }
        unitComponent.Remove(destroyUnitId);
    }
}