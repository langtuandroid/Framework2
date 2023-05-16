using System;
using Box2DSharp.Dynamics;
using Framework;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 一个碰撞体Component,包含一个碰撞实例所有信息，直接挂载到碰撞体Unit上
/// 比如诺手Q技能碰撞体UnitA，那么这个B2D_ColliderComponent的Entity就是UnitA，而其中的BelongToUnit就是诺手
/// </summary>
public class B2D_ColliderComponent : Entity, IAwakeSystem<ColliderArgs> , IUpdateSystem, IDestroySystem
{
    public B2D_WorldComponent WorldComponent;

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
    /// 是否同步归属的UnitPos
    /// </summary>
    public bool SyncPosToBelongUnit;

    public float3 Offset;

    /// <summary>
    /// 是否同步归属的UnitRot
    /// </summary>
    public bool SyncRotToBelongUnit;

    public float Angle;

    /// <summary>
    /// 碰撞体数据实例
    /// </summary>
    public B2D_ColliderDataStructureBase B2D_ColliderDataStructureBase;

    private object userData;

    private Transform HangPoint;
    private Unit selfUnit;

    
    public void Awake(ColliderArgs args)
    {
        CollisionHandlerName = args.CollisionHandlerName ?? string.Empty;
        BelongToUnit = args.BelongToUnit;
        SyncRotToBelongUnit = args.SyncRot;
        SyncPosToBelongUnit = args.SyncPos;
        B2D_ColliderDataStructureBase = args.ColliderDataStructureBase;
        userData = args.UserData;
        
        selfUnit = GetParent<Unit>();
        HangPoint = BelongToUnit.GetComponent<GameObjectComponent>()?.Find(args.HangPoint);
        Offset = args.Offset;
        Angle = args.Angle;
        if (HangPoint != null)
        {
            selfUnit.Position = HangPoint.position.ToFloat3() + args.Offset;
            selfUnit.EulerAngle = new float3(0, HangPoint.eulerAngles.y + Angle, 0);
        }
        else
        {
            selfUnit.Position = BelongToUnit.Position + args.Offset;
            selfUnit.EulerAngle = new float3(0, BelongToUnit.EulerAngle.y + Angle, 0);
        }

        WorldComponent = this.DomainScene().GetComponent<B2D_WorldComponent>();

        Body = WorldComponent.CreateDynamicBody();
        B2D_ColliderDataLoadHelper.ApplyFixture(B2D_ColliderDataStructureBase, Body,
            AddChild<ColliderUserData, Unit, object>(GetParent<Unit>(), args.UserData));
        SyncBody(); 
    }
    
    /// <summary>
    /// 同步刚体（依据Unit载体，例如诺克UnitA释放碰撞体UnitB，这里的Unit同步是UnitB的同步）
    /// </summary>
    /// <param name="self"></param>
    /// <param name="pos"></param>
    public void SyncBody()
    {
        Body.SetTransform(new System.Numerics.Vector2(selfUnit.Position.x, selfUnit.Position.z), Body.GetAngle());
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
            if (HangPoint == null)
            {
                selfUnit.Position = BelongToUnit.Position + Offset;
            }
            else
            {
                selfUnit.Position = HangPoint.position.ToFloat3() + Offset;
            }
        }

        if (SyncPosToBelongUnit)
        {
            if (HangPoint == null)
            {
                selfUnit.EulerAngle = BelongToUnit.EulerAngle + new float3(0, Angle, 0);
            }
            else
            {
                selfUnit.EulerAngle = HangPoint.eulerAngles.ToFloat3() + new float3(0, Angle, 0);
            }
        }

        if (SyncPosToBelongUnit || SyncRotToBelongUnit)
        {
            SyncBody();
        }
    }

    public void OnDestroy()
    {
        this.DomainScene().GetComponent<B2D_WorldComponent>().AddBodyTobeDestroyed(Body);
    }

}

public class ColliderArgs : IReference
{
    public string CollisionHandlerName;
    public Unit BelongToUnit;
    public string HangPoint;
    public float3 Offset;
    public float Angle;
    public bool SyncPos;
    public bool SyncRot;
    public B2D_ColliderDataStructureBase ColliderDataStructureBase;
    public object UserData;

    public void Clear()
    {
        HangPoint = default;
        CollisionHandlerName = default;
        BelongToUnit = default;
        Offset = default;
        Angle = default;
        SyncPos = default;
        SyncRot = default;
        ColliderDataStructureBase = default;
        UserData = default;
    }
}