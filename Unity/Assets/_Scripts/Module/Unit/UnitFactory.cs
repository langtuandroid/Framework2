using Framework;
using UnityEngine;

public class UnitFactory
{
    public static Unit CreateUnit(Scene scene, int unitId)
    {
        UnitComponent unitComponent = scene.GetComponent<UnitComponent>();

        Unit unit = unitComponent.AddChild<Unit,int>(unitId);

        unitComponent.Add(unit);

        return unit;
    }

    public static Unit CreateHero(Scene scene, RoleCamp roleCamp, int heroConfigId)
    {
        Unit unit = CreateUnit(scene,heroConfigId);
        unit.AddComponent<NP_SyncComponent>();
        unit.AddComponent<NumericComponent>();
        unit.AddComponent<MoveComponent>();

        //增加Buff管理组件
        unit.AddComponent<BuffManagerComponent>();
        unit.AddComponent<B2D_RoleCastComponent, RoleCamp, RoleTag>(roleCamp, RoleTag.Hero);

        unit.AddComponent<NP_RuntimeTreeManager>();
        //Log.Info("行为树创建完成");
        unit.AddComponent<ObjectWait>();
        unit.AddComponent<CastDamageComponent>();
        unit.AddComponent<ReceiveDamageComponent>();

        unit.AddComponent<GameObjectComponent>();
        var colliderRadius = UnitConfigFactory.Instance.Get(heroConfigId).ColliderRadius;
        var colliderUnit = CreateUnit(scene, 0);
        var colliderArgs = ReferencePool.Allocate<ColliderArgs>();
        colliderArgs.BelongToUnit = unit;
        colliderArgs.SyncPos = true;
        colliderArgs.SyncRot = true;
        colliderArgs.ColliderDataStructureBase = new B2D_CircleColliderDataStructure()
            { B2D_ColliderType = B2D_ColliderType.CircleCollider, isSensor = true, radius = colliderRadius };
        colliderUnit.AddComponent<B2D_ColliderComponent, ColliderArgs>(colliderArgs); 
        return unit;
    }
    
    
    public static Unit CreateSoldier(Scene scene, RoleCamp roleCamp, int heroConfigId)
    {
        Unit unit = CreateUnit(scene, heroConfigId);
        unit.AddComponent<NP_SyncComponent>();
        unit.AddComponent<NumericComponent>();
        unit.AddComponent<MoveComponent>();

        //增加Buff管理组件
        unit.AddComponent<BuffManagerComponent>();
        unit.AddComponent<B2D_RoleCastComponent, RoleCamp, RoleTag>(roleCamp, RoleTag.Hero);

        unit.AddComponent<NP_RuntimeTreeManager>();
        //Log.Info("行为树创建完成");
        unit.AddComponent<ObjectWait>();
        unit.AddComponent<CastDamageComponent>();
        unit.AddComponent<ReceiveDamageComponent>();

        unit.AddComponent<GameObjectComponent>();

        var colliderRadius = UnitConfigFactory.Instance.Get(heroConfigId).ColliderRadius;
        var colliderUnit = CreateUnit(scene, 0);
        var colliderArgs = ReferencePool.Allocate<ColliderArgs>();
        colliderArgs.BelongToUnit = unit;
        colliderArgs.SyncPos = true;
        colliderArgs.SyncRot = true;
        colliderArgs.ColliderDataStructureBase = new B2D_CircleColliderDataStructure()
            { B2D_ColliderType = B2D_ColliderType.CircleCollider, isSensor = true, radius = colliderRadius };
        colliderUnit.AddComponent<B2D_ColliderComponent, ColliderArgs>(colliderArgs);
        return unit;
    }

    /// <summary>
    /// 创建碰撞体
    /// </summary>
    /// <param name="room">归属的房间</param>
    /// <param name="belongToUnit">归属的Unit</param>
    /// <param name="colliderDataConfigId">碰撞体数据表Id</param>
    /// <param name="collisionRelationDataConfigId">碰撞关系数据表Id</param>
    /// <param name="colliderNPBehaveTreeIdInExcel">碰撞体的行为树Id</param>
    /// <returns></returns>
    public static Unit CreateSpecialColliderUnit(Scene room, long belongToUnitId, long selfId,
        int collisionRelationDataConfigId, int colliderNPBehaveTreeIdInExcel,string hangPoint, bool followUnitPos,
        bool followUnitRot, Vector3 offset,
        float angle)
    {
        //为碰撞体新建一个Unit
        Unit b2sColliderEntity = CreateUnit(room, 0);
        Unit belongToUnit = room.GetComponent<UnitComponent>().Get(belongToUnitId);
        SkillCanvasData skillCanvasData = SkillCanvasDataFactory.Instance.Get(colliderNPBehaveTreeIdInExcel);
        var colliderArgs = ReferencePool.Allocate<ColliderArgs>();
        B2D_CollisionRelationConfig serverB2SCollisionRelationConfig =
            B2D_CollisionRelationConfigFactory.Instance.Get(collisionRelationDataConfigId);
        colliderArgs.CollisionHandlerName = serverB2SCollisionRelationConfig.ColliderHandlerName;
        colliderArgs.BelongToUnit = belongToUnit;
        colliderArgs.SyncPos = followUnitPos;
        colliderArgs.SyncRot = followUnitRot;
        colliderArgs.HangPoint = hangPoint;
        colliderArgs.Offset = offset;
        colliderArgs.Angle = angle;
        B2D_ColliderDataRepositoryComponent b2DColliderDataRepositoryComponent =
            room.GetComponent<B2D_ColliderDataRepositoryComponent>();
        colliderArgs.ColliderDataStructureBase =
            b2DColliderDataRepositoryComponent.GetDataByColliderId(serverB2SCollisionRelationConfig.ID);
        b2sColliderEntity.AddComponent<NP_SyncComponent>();
        b2sColliderEntity.AddComponent<B2D_ColliderComponent, ColliderArgs>(colliderArgs);
        b2sColliderEntity.AddComponent<NP_RuntimeTreeManager>();

        //根据传过来的行为树Id来给这个碰撞Unit加上行为树
        NP_RuntimeTreeFactory
            .CreateSkillNpRuntimeTree(b2sColliderEntity, skillCanvasData.NPBehaveId)
            .Start();

        return b2sColliderEntity;
    }
    
    /// <summary>
    /// 创建碰撞体
    /// </summary>
    /// <param name="scene">归属的房间</param>
    /// <param name="belongToUnit">归属的Unit</param>
    /// <param name="colliderDataConfigId">碰撞体数据表Id</param>
    /// <param name="collisionRelationDataConfigId">碰撞关系数据表Id</param>
    /// <param name="colliderNPBehaveTreeIdInExcel">碰撞体的行为树Id</param>
    /// <returns></returns>
    public static Unit CreateDefaultColliderUnit(Scene scene, long belongToUnitId, long selfId,
        int collisionRelationDataConfigId, int colliderNPBehaveTreeIdInExcel, string hangPoint, bool followUnitPos,
        bool followUnitRot, Vector3 offset,
        float angle, float duration, DefaultColliderData defaultColliderData)
    {
        //为碰撞体新建一个Unit
        Unit b2sColliderEntity = CreateUnit(scene, 0);
        Unit belongToUnit = scene.GetComponent<UnitComponent>().Get(belongToUnitId);
        SkillCanvasData skillCanvasData = SkillCanvasDataFactory.Instance.Get(colliderNPBehaveTreeIdInExcel);

        b2sColliderEntity.AddComponent<NP_SyncComponent>();
        var colliderArgs = ReferencePool.Allocate<ColliderArgs>();
        B2D_CollisionRelationConfig serverB2SCollisionRelationConfig =
            B2D_CollisionRelationConfigFactory.Instance.Get(collisionRelationDataConfigId);
        colliderArgs.CollisionHandlerName = serverB2SCollisionRelationConfig.ColliderHandlerName;
        colliderArgs.BelongToUnit = belongToUnit;
        colliderArgs.SyncPos = followUnitPos;
        colliderArgs.HangPoint = hangPoint;
        colliderArgs.SyncRot = followUnitRot;
        colliderArgs.Offset = offset;
        colliderArgs.Angle = angle;
        colliderArgs.UserData = defaultColliderData;
        B2D_ColliderDataRepositoryComponent b2DColliderDataRepositoryComponent =
            scene.GetComponent<B2D_ColliderDataRepositoryComponent>();
        colliderArgs.ColliderDataStructureBase =
            b2DColliderDataRepositoryComponent.GetDataByColliderId(serverB2SCollisionRelationConfig.ID);
        b2sColliderEntity.AddComponent<B2D_ColliderComponent, ColliderArgs>(colliderArgs);
        b2sColliderEntity.AddComponent<NP_RuntimeTreeManager>();

        //根据传过来的行为树Id来给这个碰撞Unit加上行为树
        var behave = NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(b2sColliderEntity, skillCanvasData.NPBehaveId);
        behave.GetBlackboard().Set("Duration", duration);
        behave.Start();
        return b2sColliderEntity;
    } 

    public class CreateHeroColliderArgs
    {
        public Unit Unit;
        public B2D_ColliderDataStructureBase B2SColliderDataStructureBase;
        public string CollisionHandler;
        public bool FollowUnit;
    }

    public class CreateSkillColliderArgs
    {
        public Unit belongToUnit;
        public int collisionRelationDataConfigId;
        public DefaultColliderData DefaultColliderData;
        public string HangPointPath;
        public bool FollowUnitPos;
        public bool FollowUnitRot;
        public Vector3 offset;
        public float angle;
    }
}