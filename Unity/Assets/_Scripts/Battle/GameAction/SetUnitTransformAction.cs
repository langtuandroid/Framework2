using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("设置Unit的Transform信息", TitleAlignment = TitleAlignments.Centered)]
public class SetUnitTransformAction : SkillBaseAction
{
    [LabelText("是否设置位置")] public bool SetPos;

    [LabelText("是否设置旋转")] public bool SetRot;
    
    [LabelText("是否设置缩放")] public bool SetScale;

    [ShowIf(nameof(SetPos))] [LabelText("将要设置的位置")]
    public NP_BlackBoardRelationData<Vector3> PosBlackBoardRelationData = new ();

    [ShowIf(nameof(SetRot))] [LabelText("将要设置的旋转")]
    public NP_BlackBoardRelationData<float> RotBlackBoardRelationData = new ();
    
    [ShowIf(nameof(SetScale))] [LabelText("将要设置的缩放")]
    public NP_BlackBoardRelationData<Vector3> ScaleBlackBoardRelationData = new ();

    public override Action GetActionToBeDone()
    {
        return SetUnitTransformAction;
    }

    private void SetUnitTransformAction()
    {
        Unit unit = BelongToUnit;
        if (SetPos)
        {
            Vector3 result = PosBlackBoardRelationData.GetBlackBoardValue(
                this.BelongtoRuntimeTree
                    .GetBlackboard());
            unit.Position = result;
            unit.GetComponent<GameObjectComponent>().GameObject.transform.position = result;
        }

        if (SetRot)
        {
            unit.Rotation = Quaternion.Euler(0,
                RotBlackBoardRelationData.GetBlackBoardValue(this.BelongtoRuntimeTree.GetBlackboard()), 0);
        }

        if (SetScale)
        {
             Vector3 result = ScaleBlackBoardRelationData.GetBlackBoardValue(
                 this.BelongtoRuntimeTree
                     .GetBlackboard());
             unit.Scale = result;
        }
    }
}