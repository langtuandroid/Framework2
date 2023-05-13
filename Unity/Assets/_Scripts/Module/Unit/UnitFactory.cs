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

    public static Unit CreateHero(Scene scene, RoleCamp roleCamp, int heroConfigId)
    {
        Unit unit = CreateUnit(scene);
        unit.AddComponent<NP_SyncComponent>();
        unit.AddComponent<NumericComponent>();
        unit.AddComponent<MoveComponent>();

        //增加Buff管理组件
        unit.AddComponent<BuffManagerComponent>();
        unit.AddComponent<SkillCanvasManagerComponent>();
        unit.AddComponent<B2D_RoleCastComponent, RoleCamp, RoleTag>(roleCamp, RoleTag.Hero);

        unit.AddComponent<NP_RuntimeTreeManager>();
        //Log.Info("行为树创建完成");
        unit.AddComponent<ObjectWait>();
        unit.AddComponent<CastDamageComponent>();
        unit.AddComponent<ReceiveDamageComponent>();

        unit.AddComponent<GameObjectComponent>();

        var colliderRadius = HeroConfigFactory.Instance.Get(heroConfigId).ColliderRadius;
        var colliderUnit = CreateUnit(scene);
        colliderUnit.AddComponent<B2D_ColliderComponent, CreateHeroColliderArgs>(new CreateHeroColliderArgs
        {
            B2SColliderDataStructureBase = new B2D_CircleColliderDataStructure()
                { B2D_ColliderType = B2D_ColliderType.CircleCollider, isSensor = true, radius = colliderRadius },
            FollowUnit = true, Unit = unit
        });

        return unit;
    }
    
    
    public static Unit CreateSoldier(Scene scene, RoleCamp roleCamp, int heroConfigId)
    {
        Unit unit = CreateUnit(scene);
        unit.AddComponent<NP_SyncComponent>();
        unit.AddComponent<NumericComponent>();
        unit.AddComponent<MoveComponent>();

        //增加Buff管理组件
        unit.AddComponent<BuffManagerComponent>();
        unit.AddComponent<SkillCanvasManagerComponent>();
        unit.AddComponent<B2D_RoleCastComponent, RoleCamp, RoleTag>(roleCamp, RoleTag.Hero);

        unit.AddComponent<NP_RuntimeTreeManager>();
        //Log.Info("行为树创建完成");
        unit.AddComponent<ObjectWait>();
        unit.AddComponent<CastDamageComponent>();
        unit.AddComponent<ReceiveDamageComponent>();

        unit.AddComponent<GameObjectComponent>();

        var colliderRadius = SoldierConfigFactory.Instance.Get(heroConfigId).ColliderRadius;
        var colliderUnit = CreateUnit(scene);
        colliderUnit.AddComponent<B2D_ColliderComponent, CreateHeroColliderArgs>(new CreateHeroColliderArgs
        {
            B2SColliderDataStructureBase = new B2D_CircleColliderDataStructure()
                { B2D_ColliderType = B2D_ColliderType.CircleCollider, isSensor = true, radius = colliderRadius },
            FollowUnit = true, Unit = unit
        });

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
        Unit b2sColliderEntity = CreateUnit(room);
        Unit belongToUnit = room.GetComponent<UnitComponent>().Get(belongToUnitId);
        SkillCanvasData skillCanvasData = SkillCanvasDataFactory.Instance.Get(colliderNPBehaveTreeIdInExcel);

        b2sColliderEntity.AddComponent<NP_SyncComponent>();
        b2sColliderEntity.AddComponent<B2D_ColliderComponent, CreateSkillColliderArgs>(
            new CreateSkillColliderArgs()
            {
                belongToUnit = belongToUnit, collisionRelationDataConfigId = collisionRelationDataConfigId,
                FollowUnitPos = followUnitPos, FollowUnitRot = followUnitRot, offset = offset,
                HangPointPath = hangPoint, angle = angle
            });
        b2sColliderEntity.AddComponent<NP_RuntimeTreeManager>();
        b2sColliderEntity.AddComponent<SkillCanvasManagerComponent>();

        //根据传过来的行为树Id来给这个碰撞Unit加上行为树
        NP_RuntimeTreeFactory
            .CreateSkillNpRuntimeTree(b2sColliderEntity, skillCanvasData.NPBehaveId, skillCanvasData.BelongToSkillId)
            .Start();

        return b2sColliderEntity;
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
    public static Unit CreateDefaultColliderUnit(Scene room, long belongToUnitId, long selfId,
        int collisionRelationDataConfigId, int colliderNPBehaveTreeIdInExcel, int skillId,string hangPoint, bool followUnitPos,
        bool followUnitRot, Vector3 offset,
        float angle, float duration, DefaultColliderData defaultColliderData)
    {
        //为碰撞体新建一个Unit
        Unit b2sColliderEntity = CreateUnit(room);
        Unit belongToUnit = room.GetComponent<UnitComponent>().Get(belongToUnitId);
        SkillCanvasData skillCanvasData = SkillCanvasDataFactory.Instance.Get(colliderNPBehaveTreeIdInExcel);

        b2sColliderEntity.AddComponent<NP_SyncComponent>();
        b2sColliderEntity.AddComponent<B2D_ColliderComponent, CreateSkillColliderArgs>(
            new CreateSkillColliderArgs()
            {
                belongToUnit = belongToUnit, collisionRelationDataConfigId = collisionRelationDataConfigId,
                FollowUnitPos = followUnitPos, FollowUnitRot = followUnitRot, offset = offset,
                HangPointPath = hangPoint, angle = angle, DefaultColliderData =  defaultColliderData
            });
        b2sColliderEntity.AddComponent<NP_RuntimeTreeManager>();
        b2sColliderEntity.AddComponent<SkillCanvasManagerComponent>();

        //根据传过来的行为树Id来给这个碰撞Unit加上行为树
        var behave = NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(b2sColliderEntity, skillCanvasData.NPBehaveId, skillId);
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