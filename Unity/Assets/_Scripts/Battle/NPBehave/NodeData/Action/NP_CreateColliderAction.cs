using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 创建碰撞体Action
/// 创建碰撞体会创建一个UnitA作为碰撞体的载体，然后为其添加碰撞相关组件让其变成一个游戏中的碰撞体
/// 碰撞体一般都会有一个BelongToUnit，指向释放者
/// Box2D碰撞体每帧都会同步UnitA的Transform信息作为自己在Box2D世界中的Transform信息
/// 所以FollowUnit选项只是让我们决定UnitA的Transform是否每帧由BelongToUnit决定
/// </summary>
[Title("创建一个碰撞体", TitleAlignment = TitleAlignments.Centered)]
public class NP_CreateColliderAction : NP_ClassForStoreAction
{
    [LabelText("碰撞关系配置Id")] [Tooltip("Excel配置表中Id")]
    [Required] public VTD_ColliderReleationId ColliderReleationsRelationSupportIdInExcel = new VTD_ColliderReleationId();
    
    [LabelText("行为树配置表Id")] [Tooltip("Excel配置表中Id")]
    public int ColliderNPBehaveTreeIdInExcel; 

    [LabelText("生成点")] public string HangPoint;

    /// <summary>
    /// 比如诺克释放了Q技能，这里如果为True，Q技能的碰撞体就会跟随诺克
    /// </summary>
    [LabelText("Pos是否跟随释放的Unit")] public bool FollowUnitPos;

    [LabelText("Rot是否跟随释放的Unit")] public bool FollowUnitRot;

    [LabelText("相对于释放者的偏移量")]
    public Vector3 Offset;

    [LabelText("相对于释放者的旋转角度")]
    public float Angle;

    public override Action GetActionToBeDone()
    {
        this.Action = this.CreateColliderData;
        return this.Action;
    }

    private void CreateColliderData()
    {
        int colliderDataConfigId = B2D_CollisionRelationConfigFactory.Instance
            .Get(ColliderReleationsRelationSupportIdInExcel.Value)
            .ColliderConfigId;

       UnitFactory.CreateSpecialColliderUnit(BelongToUnit.DomainScene(), BelongToUnit.Id, colliderDataConfigId,
                ColliderReleationsRelationSupportIdInExcel.Value, ColliderNPBehaveTreeIdInExcel,HangPoint, FollowUnitPos, FollowUnitRot,
                Offset, Angle);
    }
}