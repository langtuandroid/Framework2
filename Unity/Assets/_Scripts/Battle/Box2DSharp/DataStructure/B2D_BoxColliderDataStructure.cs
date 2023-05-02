using Sirenix.OdinInspector;

/// <summary>
/// 矩形碰撞体的数据结构
/// </summary>
public class B2D_BoxColliderDataStructure : B2D_ColliderDataStructureBase
{
    [LabelText("x轴方向上的一半长度")] [DisableInEditorMode]
    public float hx;

    [LabelText("y轴方向上的一半长度")] [DisableInEditorMode]
    public float hy;
}