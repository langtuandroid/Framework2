using Sirenix.OdinInspector;
using Unity.Mathematics;

public enum B2D_ColliderType
{
    [LabelText("矩形碰撞体")] BoxColllider,

    [LabelText("圆形碰撞体")] CircleCollider,

    [LabelText("多边形碰撞体")] PolygonCollider,
}

public class B2D_ColliderDataStructureBase
{
    [LabelText("碰撞体ID")] public long id;

    [LabelText("是否为触发器")] public bool isSensor;

    [LabelText("Box2D碰撞体类型")] public B2D_ColliderType B2D_ColliderType;

    [LabelText("碰撞体偏移信息")] public float2 finalOffset = new float2(0, 0);
}