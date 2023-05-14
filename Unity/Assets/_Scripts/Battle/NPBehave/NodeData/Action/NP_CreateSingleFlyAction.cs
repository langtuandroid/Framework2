using System;
using Sirenix.OdinInspector;

[Title("创建单一飞行物", TitleAlignment = TitleAlignments.Centered)]
public class NP_CreateSingleFlyAction : NP_ClassForStoreAction
{

    [LabelText("飞行物prefab")]
    public string FlyPath;

    [LabelText("飞行物出生点")]
    public string HangPoint;

    [LabelText("飞行速度")]
    public float Speed;
    
    public bool 
    
    public override Action GetActionToBeDone()
    {
        this.Action = this.Create;
        return this.Action;
    }

    private void Create()
    {
    }
}