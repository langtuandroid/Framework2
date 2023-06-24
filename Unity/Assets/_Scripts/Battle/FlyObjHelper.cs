using Framework;
using Unity.Mathematics;

public static class FlyObjHelper
{
    public static void CreateSingleFlyObj(NP_CreateSingleFlyAction action)
    {
        Scene scene = action.BelongToUnit.DomainScene();
        //为碰撞体新建一个Unit
        // Unit flyObj = UnitFactory.CreateUnit(scene);
        // SkillCanvasData skillCanvasData = SkillCanvasDataFactory.Instance.Get(colliderNPBehaveTreeIdInExcel);
        // flyObj.AddComponent<NP_SyncComponent>();
        // flyObj.AddComponent<B2D_ColliderComponent, UnitFactory.CreateSkillColliderArgs>(
        //     new UnitFactory.CreateSkillColliderArgs()
        //     {
        //         belongToUnit = belongToUnit, collisionRelationDataConfigId = collisionRelationDataConfigId,
        //         FollowUnitPos = followUnitPos, FollowUnitRot = followUnitRot, offset = offset,
        //         HangPointPath = hangPoint, angle = angle
        //     });
        /*flyObj.AddComponent<NP_RuntimeTreeManager>();
        flyObj.AddComponent<SkillCanvasManagerComponent>();

        //根据传过来的行为树Id来给这个碰撞Unit加上行为树
        NP_RuntimeTreeFactory
            .CreateSkillNpRuntimeTree(flyObj, skillCanvasData.NPBehaveId, skillCanvasData.BelongToSkillId)
            .Start();

        return flyObj;*/
    }
}