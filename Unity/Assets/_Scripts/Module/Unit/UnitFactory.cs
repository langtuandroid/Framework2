using System.Threading.Tasks;
using Framework;
using UnityEngine;

public class UnitFactory
{
    public static Unit CreateUnit(Scene scene)
    {
        UnitComponent unitComponent = scene.GetComponent<UnitComponent>();

        Unit unit = unitComponent.AddChild<Unit>();

        unitComponent.Add(unit);

        return unit;
    }

    public static Unit CreateHero(Scene scene, RoleCamp roleCamp)
    {
        Unit unit = CreateUnit(scene);
        unit.AddComponent<NP_SyncComponent>();
        unit.AddComponent<NumericComponent>();
        unit.AddComponent<MoveComponent>();
        unit.AddComponent<SkillManagerComponent>();

        //增加Buff管理组件
        unit.AddComponent<BuffManagerComponent>();
        unit.AddComponent<RoleCastComponent, RoleCamp, RoleTag>(roleCamp, RoleTag.Hero);

        unit.AddComponent<NP_RuntimeTreeManager>();
        //Log.Info("行为树创建完成");
        unit.AddComponent<ObjectWait>();
        unit.AddComponent<CastDamageComponent>();
        unit.AddComponent<ReceiveDamageComponent>();
        unit.AddComponent<PlayAnimComponent>();

        unit.AddComponent<FindTargetComponent>();
        unit.AddComponent<GameObjectComponent, bool, bool>(false, true);
        ColliderArgs colliderArgs = ReferencePool.Allocate<ColliderArgs>();
        colliderArgs.BelongToUnit = unit;
        unit.AddComponent<ColliderComponent, ColliderArgs>(colliderArgs);
        return unit;
    }

    public static Unit CreateTower(Scene scene)
    {
        Unit unit = CreateUnit(scene);
        unit.AddComponent<NP_SyncComponent>();
        unit.AddComponent<NumericComponent>();
        unit.AddComponent<SkillManagerComponent>();

        //增加Buff管理组件
        unit.AddComponent<BuffManagerComponent>();
        unit.AddComponent<RoleCastComponent, RoleCamp, RoleTag>(RoleCamp.self, RoleTag.Tower);

        unit.AddComponent<NP_RuntimeTreeManager>();
        //Log.Info("行为树创建完成");
        unit.AddComponent<ObjectWait>();
        unit.AddComponent<CastDamageComponent>();
        unit.AddComponent<PlayAnimComponent>();

        unit.AddComponent<FindTargetComponent>();
        unit.AddComponent<GameObjectComponent, bool, bool>(false, true);
        return unit;
    }


    public static Unit CreateSoldier(Scene scene, RoleCamp roleCamp)
    {
        Unit unit = CreateUnit(scene);
        unit.AddComponent<NP_SyncComponent>();
        unit.AddComponent<NumericComponent>();
        // unit.AddComponent<MoveComponent>();

        //增加Buff管理组件
        unit.AddComponent<BuffManagerComponent>();
        unit.AddComponent<RoleCastComponent, RoleCamp, RoleTag>(roleCamp, RoleTag.Hero);

        unit.AddComponent<NP_RuntimeTreeManager>();
        //Log.Info("行为树创建完成");
        unit.AddComponent<ObjectWait>();
        unit.AddComponent<CastDamageComponent>();
        unit.AddComponent<ReceiveDamageComponent>();

        unit.AddComponent<GameObjectComponent, bool, bool>(true, false);

        ColliderArgs colliderArgs = ReferencePool.Allocate<ColliderArgs>();
        colliderArgs.BelongToUnit = unit;
        unit.AddComponent<ColliderComponent, ColliderArgs>(colliderArgs);
        return unit;
    }

    /// <summary>
    /// 创建碰撞体
    /// </summary>
    /// <returns></returns>
    public static Unit CreateSpecialColliderUnit(Scene room, long belongToUnitId,
        int colliderNpBehaveTreeIdInExcel, string hangPoint, bool followUnitPos,
        bool followUnitRot, Vector3 offset,
        float angle)
    {
        //为碰撞体新建一个Unit
        Unit b2SColliderEntity = CreateUnit(room);
        Unit belongToUnit = room.GetComponent<UnitComponent>().Get(belongToUnitId);
        ColliderArgs colliderArgs = ReferencePool.Allocate<ColliderArgs>();
        colliderArgs.CollisionHandlerName = "";
        colliderArgs.BelongToUnit = belongToUnit;
        b2SColliderEntity.AddComponent<NP_SyncComponent>();
        b2SColliderEntity.AddComponent<ColliderComponent, ColliderArgs>(colliderArgs);
        b2SColliderEntity.AddComponent<NP_RuntimeTreeManager>();

        //根据传过来的行为树Id来给这个碰撞Unit加上行为树
        NP_RuntimeTreeFactory
            .CreateBehaveRuntimeTree(b2SColliderEntity, colliderNpBehaveTreeIdInExcel)
            .Start();

        return b2SColliderEntity;
    }

    public static Unit CreateNormalDefaultColliderUnit(Scene scene, GameObject collider, long belongToUnitId,
        float duration, bool needDestroyCollider,
        NormalDefaultColliderData defaultColliderData)
    {
        Unit belongToUnit = scene.GetComponent<UnitComponent>().Get(belongToUnitId);
        ColliderArgs colliderArgs = ReferencePool.Allocate<ColliderArgs>();
        colliderArgs.CollisionHandlerName = nameof(NormalDefaultCollisionHandler);
        colliderArgs.BelongToUnit = belongToUnit;
        colliderArgs.UserData = defaultColliderData;
        //为碰撞体新建一个Unit
        Unit unit = CreateCollider(scene, collider.gameObject, 10000, colliderArgs,
            out NP_RuntimeTree behave);
        //根据传过来的行为树Id来给这个碰撞Unit加上行为树
        behave.GetBlackboard().Set("Duration [10000]", duration);
        behave.GetBlackboard().Set("NeedDestroyCollider [10000]", needDestroyCollider);
        behave.Start();
        return unit;
    }


    public static Unit CreateBehaveDefaultColliderUnit(Scene scene, long colliderUnitId, long belongToUnitId,
        float duration, BehaveDefaultColliderData behaveDefaultColliderData)
    {
        Unit colliderUnit = scene.GetComponent<UnitComponent>().Get(colliderUnitId);
        return CreateBehaveDefaultColliderUnit(scene, colliderUnit.GetComponent<GameObjectComponent>().GameObject,
            belongToUnitId, duration,
            false, behaveDefaultColliderData);
    }

    public static Unit CreateBehaveDefaultColliderUnit(Scene scene, GameObject collider, long belongToUnitId,
        float duration, bool needDestroyCollider,
        BehaveDefaultColliderData behaveDefaultColliderData)
    {
        Unit belongToUnit = scene.GetComponent<UnitComponent>().Get(belongToUnitId);
        ColliderArgs colliderArgs = ReferencePool.Allocate<ColliderArgs>();
        colliderArgs.CollisionHandlerName = nameof(BehaveDefaultCollisionHandler);
        colliderArgs.BelongToUnit = belongToUnit;
        colliderArgs.UserData = behaveDefaultColliderData;
        //为碰撞体新建一个Unit
        Unit unit = CreateCollider(scene, collider.gameObject, 10000, colliderArgs,
            out NP_RuntimeTree behave);
        //根据传过来的行为树Id来给这个碰撞Unit加上行为树
        behave.GetBlackboard().Set("Duration [10000]", duration);
        behave.GetBlackboard().Set("NeedDestroyCollider [10000]", needDestroyCollider);
        behave.Start();
        return unit;
    }

    public static Unit CreateCollider(Scene scene, GameObject collider, int colliderNpBehaveTreeIdInExcel,
        ColliderArgs colliderArgs, out NP_RuntimeTree runtimeTree)
    {
        //为碰撞体新建一个Unit
        Unit colliderEntity = CreateUnit(scene);
        colliderEntity.AddComponent<NP_SyncComponent>();
        colliderEntity.AddComponent<GameObjectComponent, bool, bool>(true, false).GameObject = collider;
        colliderEntity.AddComponent<ColliderComponent, ColliderArgs>(colliderArgs);
        colliderEntity.AddComponent<NP_RuntimeTreeManager>();

        //根据传过来的行为树Id来给这个碰撞Unit加上行为树
        runtimeTree = NP_RuntimeTreeFactory.CreateBehaveRuntimeTree(colliderEntity, colliderNpBehaveTreeIdInExcel);
        return colliderEntity;
    }
}