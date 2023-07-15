using System;
using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("添加默认碰撞", TitleAlignment = TitleAlignments.Centered)]
public class NP_CreateDefaultColliderAction : NP_ClassForStoreAction
{

    [LabelText("带碰撞的UnitId")] public BlackboardOrValue_Long ColliderUnit = new BlackboardOrValue_Long();

    [LabelText("持续时间")] public BlackboardOrValue_Float Duration = new();

    public DefaultColliderNode DefaultColliderNode = new DefaultColliderNode();
    
    public override Action GetActionToBeDone()
    {
        this.Action = this.CreateColliderData;
        return this.Action;
    }

    private void CreateColliderData()
    {
        UnitFactory.CreateDefaultColliderUnit(BelongToUnit.DomainScene(),
            ColliderUnit.GetValue(BelongtoRuntimeTree.GetBlackboard()), BelongToUnit.Id,
            Duration.GetValue(BelongtoRuntimeTree.GetBlackboard()),
            DefaultColliderNode.ToColliderData(BelongtoRuntimeTree.GetBlackboard()));
    }
}