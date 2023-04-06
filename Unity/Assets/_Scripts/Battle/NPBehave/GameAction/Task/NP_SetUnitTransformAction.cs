using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("设置Unit的Transform信息", TitleAlignment = TitleAlignments.Centered)]
public class NP_SetUnitTransformAction : NP_ClassForStoreAction
{
    [LabelText("是否设置位置")] public bool SetPos;

    [LabelText("是否设置旋转")] public bool SetRot;
    
    [LabelText("是否设置缩放")] public bool SetScale;

    [ShowIf(nameof(SetPos))] [LabelText("将要设置的位置")]
    public NP_BlackBoardRelationData PosBlackBoardRelationData = new NP_BlackBoardRelationData();

    [ShowIf(nameof(SetRot))] [LabelText("将要设置的旋转")]
    public NP_BlackBoardRelationData RotBlackBoardRelationData = new NP_BlackBoardRelationData();
    
    [ShowIf(nameof(SetScale))] [LabelText("将要设置的缩放")]
    public NP_BlackBoardRelationData ScaleBlackBoardRelationData = new NP_BlackBoardRelationData();

    public override Action GetActionToBeDone()
    {
        this.Action = this.SetUnitTransformAction;
        return this.Action;
    }

    private void SetUnitTransformAction()
    {
        Unit unit = BelongToUnit;
        if (SetPos)
        {
            Vector3 result = PosBlackBoardRelationData.GetBlackBoardValue<Vector3>(
                this.BelongtoRuntimeTree
                    .GetBlackboard());
            unit.Position = result;
            unit.GetComponent<GameObjectComponent>().GameObject.transform.position = result;
        }

        if (SetRot)
        {
            unit.Rotation = Quaternion.Euler(0,
                RotBlackBoardRelationData.GetBlackBoardValue<float>(this.BelongtoRuntimeTree.GetBlackboard()), 0);
        }

        if (SetScale)
        {
             Vector3 result = ScaleBlackBoardRelationData.GetBlackBoardValue<Vector3>(
                 this.BelongtoRuntimeTree
                     .GetBlackboard());
             unit.Scale = result;
        }
    }
}