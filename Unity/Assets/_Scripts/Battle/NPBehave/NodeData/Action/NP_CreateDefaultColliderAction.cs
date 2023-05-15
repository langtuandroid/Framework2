using System;
using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("添加默认碰撞", TitleAlignment = TitleAlignments.Centered)]
public class NP_CreateDefaultColliderAction : NP_ClassForStoreAction
{

    [LabelText("碰撞体id")] public int ColliderId;

    [LabelText("持续时间")] public float Duration;
    
    [LabelText("生成点")] public string HangPoint;

    [LabelText("碰撞的标签")] public RoleTag RoleTag = RoleTag.Hero | RoleTag.Soldier;
    
    [LabelText("碰撞的阵营")] public RoleCast RoleCast = RoleCast.Adverse;

    [LabelText("是否碰撞到的字典key")] public NP_BlackBoardRelationData<bool> HasHitKey = new ();
    
    [LabelText("碰撞到的所有物体字典key")] public NP_BlackBoardRelationData<List<long>> HitUnitsKey = new ();

    [LabelText("Pos是否跟随释放的Unit")] public bool FollowUnitPos = true;

    [LabelText("Rot是否跟随释放的Unit")] public bool FollowUnitRot = true;
    
    /// <summary>
    /// 只在跟随Unit时有效，因为不跟随Unit说明是世界空间的碰撞体，
    /// </summary>
    [LabelText("相对于释放者的偏移量")] public Vector3 Offset;

    /// <summary>
    /// 只在不跟随Unit时有效，跟随Unit代表使用BelongToUnit的Transform
    /// </summary>
    [LabelText("相对于释放者的旋转角度")] public float Angle;

    public override Action GetActionToBeDone()
    {
        this.Action = this.CreateColliderData;
        return this.Action;
    }

    private void CreateColliderData()
    {
        Log.Msg("创建了碰撞体");
        // BelongtoRuntimeTree.BelongNP_DataSupportor.
        UnitFactory.CreateDefaultColliderUnit(BelongToUnit.DomainScene(), BelongToUnit.Id, ColliderId,
            10001,10000, 
            BelongtoRuntimeTree.BelongNP_DataSupportor.ExcelId, HangPoint, FollowUnitPos, FollowUnitRot,
            Offset, Angle, Duration, new DefaultColliderData(BelongtoRuntimeTree.BelongNP_DataSupportor.ExcelId, RoleTag, RoleCast, HitUnitsKey.BBKey, HasHitKey.BBKey));
    }
}