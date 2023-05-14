using System;
using Box2DSharp.Dynamics;
using Framework;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 一个碰撞体Component,包含一个碰撞实例所有信息，直接挂载到碰撞体Unit上
/// 比如诺手Q技能碰撞体UnitA，那么这个B2D_ColliderComponent的Entity就是UnitA，而其中的BelongToUnit就是诺手
/// </summary>
public class B2D_ColliderComponent : Entity, IAwakeSystem<UnitFactory.CreateSkillColliderArgs> , IAwakeSystem<UnitFactory.CreateHeroColliderArgs>, IUpdateSystem, IDestroySystem
{
    public B2D_WorldComponent WorldComponent;

    /// <summary>
    /// 碰撞关系表中的Id (Excel中的Id)
    /// </summary>
    public int B2D_CollisionRelationConfigId;

    /// <summary>
    /// 碰撞处理者名称
    /// </summary>
    public string CollisionHandlerName;

    /// <summary>
    /// Box2D世界中的刚体
    /// </summary>
    public Body Body;

    /// <summary>
    /// 所归属的Unit，也就是产出碰撞体的Unit，
    /// 比如诺克放一个Q，那么BelongUnit就是诺克
    /// 需要注意的是，如果这个碰撞体需要同步位置，同步目标是Parent，而不是这个BelongToUnit
    /// </summary>
    public Unit BelongToUnit;

    /// <summary>
    ///比如诺克放一个Q，那么BelongToSkillConfigId就是技能的配置表id
    /// </summary>
    public DefaultColliderData DefaultColliderData;

    /// <summary>
    /// 是否同步归属的UnitPos
    /// </summary>
    public bool SyncPosToBelongUnit;

    /// <summary>
    /// 是否同步归属的UnitRot
    /// </summary>
    public bool SyncRotToBelongUnit;

    /// <summary>
    /// 碰撞体数据实例
    /// </summary>
    public B2D_ColliderDataStructureBase B2D_ColliderDataStructureBase = new B2D_ColliderDataStructureBase();

    public void Awake(UnitFactory.CreateSkillColliderArgs createSkillColliderArgs)
    {
        B2D_CollisionRelationConfig serverB2SCollisionRelationConfig =
            B2D_CollisionRelationConfigFactory.Instance
                .Get(createSkillColliderArgs.collisionRelationDataConfigId);
        string collisionHandlerName = serverB2SCollisionRelationConfig.ColliderHandlerName;

        WorldComponent = GetParent<Unit>().DomainScene().GetComponent<B2D_WorldComponent>();
        BelongToUnit = createSkillColliderArgs.belongToUnit;
        B2D_CollisionRelationConfigId = serverB2SCollisionRelationConfig.ID;
        CollisionHandlerName = collisionHandlerName ?? string.Empty;

        SyncPosToBelongUnit = createSkillColliderArgs.FollowUnitPos;
        SyncRotToBelongUnit = createSkillColliderArgs.FollowUnitRot;
        DefaultColliderData = createSkillColliderArgs.DefaultColliderData;

        Unit selfUnit = GetParent<Unit>();
        Transform hangPoint = BelongToUnit.GetComponent<GameObjectComponent>()
            .Find(createSkillColliderArgs.HangPointPath);
        selfUnit.Position = hangPoint.position + createSkillColliderArgs.offset;
        selfUnit.Rotation = math.mul(BelongToUnit.Rotation, hangPoint.rotation);

        this.CreateB2D_Collider();
        SyncBody();
    }
    
    public void Awake(UnitFactory.CreateHeroColliderArgs args)
    {
        WorldComponent = this.DomainScene().GetComponent<B2D_WorldComponent>();
        BelongToUnit = args.Unit;
        SyncPosToBelongUnit = args.FollowUnit;
        CollisionHandlerName = args.CollisionHandler ?? string.Empty;
        B2D_ColliderDataStructureBase = args.B2SColliderDataStructureBase;
        Body = WorldComponent.CreateDynamicBody();
        SyncPosToBelongUnit = true;
        SyncRotToBelongUnit = true;
        B2D_ColliderDataLoadHelper.ApplyFixture(B2D_ColliderDataStructureBase, Body,
            AddChild<ColliderUserData, Unit, DefaultColliderData>(GetParent<Unit>(), DefaultColliderData));
        SyncBody();
    }

    /// <summary>
    /// 同步刚体（依据Unit载体，例如诺克UnitA释放碰撞体UnitB，这里的Unit同步是UnitB的同步）
    /// </summary>
    /// <param name="self"></param>
    /// <param name="pos"></param>
    public void SyncBody()
    {
        //Log.Info($"{new Vector2(BelongToUnit.Position.x, BelongToUnit.Position.z)}");
        Unit selfUnit = GetParent<Unit>();
        SetColliderBodyPos(new Vector2(selfUnit.Position.x, selfUnit.Position.z));
        SetColliderBodyAngle(selfUnit.Rotation.value.y * Mathf.PI / 180);
    }

    /// <summary>
    /// 设置刚体位置
    /// </summary>
    /// <param name="self"></param>
    /// <param name="pos"></param>
    public void SetColliderBodyPos(Vector2 pos)
    {
        Body.SetTransform(pos.ToVector2(), Body.GetAngle());
        //Log.Info($"位置为{Body.GetPosition()} 类型为{Body.IsSleepingAllowed}");
    }

    /// <summary>
    /// 设置刚体角度
    /// </summary>
    /// <param name="self"></param>
    /// <param name="angle"></param>
    public void SetColliderBodyAngle(float angle)
    {
        Body.SetTransform(Body.GetPosition(), angle);
    }

    /// <summary>
    /// 设置刚体状态
    /// </summary>
    /// <param name="self"></param>
    /// <param name="state"></param>
    public void SetColliderBodyState(bool state)
    {
        Body.IsEnabled = state;
    }

    public void Update(float deltaTime)
    {
        if (SyncPosToBelongUnit)
        {
            SetColliderBodyPos(new Vector2(BelongToUnit.Position.x, BelongToUnit.Position.z));
        }
    }

    public void OnDestroy()
    {
        this.DomainScene().GetComponent<B2D_WorldComponent>().AddBodyTobeDestroyed(Body);
    }
}