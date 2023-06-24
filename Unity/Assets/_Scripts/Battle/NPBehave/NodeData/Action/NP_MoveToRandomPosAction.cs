using System;
using Framework;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

[Title("移动到随机点", TitleAlignment = TitleAlignments.Centered)]
public class NP_MoveToRandomPosAction : NP_ClassForStoreAction
{
    [BoxGroup("范围")] public int XMin;

    [BoxGroup("范围")] public int YMin;

    [BoxGroup("范围")] public int XMax;

    [BoxGroup("范围")] public int YMax;

    public override Action GetActionToBeDone()
    {
        this.Action = this.MoveToRandomPos;
        return this.Action;
    }

    private void MoveToRandomPos()
    {
        var randomTarget = new float3(RandomHelper.RandomNumber(this.XMin, this.XMax), 0,
            RandomHelper.RandomNumber(this.YMin, this.YMax));

        var speed = this.BelongToUnit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
        this.BelongToUnit.GetComponent<MoveComponent>().MoveTo(randomTarget, speed);
    }
}