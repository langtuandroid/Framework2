using System.Collections.Generic;
using Box2DSharp.Dynamics;
using Framework;
using UnityEngine;

public static class B2D_ColliderDataLoadHelper
{
    /// <summary>
    /// 加载依赖数据，并且进行碰撞体的生成
    /// </summary>
    /// <param name="self"></param>
    public static void CreateB2D_Collider(this B2D_ColliderComponent self)
    {
        B2D_ColliderDataRepositoryComponent B2DColliderDataRepositoryComponent =
            self.DomainScene().GetComponent<B2D_ColliderDataRepositoryComponent>();

        self.B2D_ColliderDataStructureBase =
            B2DColliderDataRepositoryComponent.GetDataByColliderId(self.B2D_CollisionRelationConfigId);
        self.Body = self.WorldComponent.CreateDynamicBody();

        ApplyFixture(self.B2D_ColliderDataStructureBase, self.Body, self.GetParent<Unit>());
    }

    public static void ApplyFixture(B2D_ColliderDataStructureBase B2DColliderDataStructureBase, Body body,
        Unit unit)
    {
        switch (B2DColliderDataStructureBase.B2D_ColliderType)
        {
            case B2D_ColliderType.BoxColllider:
                B2D_BoxColliderDataStructure B2DBoxColliderDataStructure =
                    (B2D_BoxColliderDataStructure)B2DColliderDataStructureBase;
                body.CreateBoxFixture(B2DBoxColliderDataStructure.hx, B2DBoxColliderDataStructure.hy,
                    B2DBoxColliderDataStructure.finalOffset, 0, B2DBoxColliderDataStructure.isSensor, unit);
                break;
            case B2D_ColliderType.CircleCollider:
                B2D_CircleColliderDataStructure B2DCircleColliderDataStructure =
                    (B2D_CircleColliderDataStructure)B2DColliderDataStructureBase;
                body.CreateCircleFixture(B2DCircleColliderDataStructure.radius,
                    B2DCircleColliderDataStructure.finalOffset,
                    B2DCircleColliderDataStructure.isSensor,
                    unit);
                break;
            case B2D_ColliderType.PolygonCollider:
                B2D_PolygonColliderDataStructure B2DPolygonColliderDataStructure =
                    (B2D_PolygonColliderDataStructure)B2DColliderDataStructureBase;
                foreach (var verxtPoint in B2DPolygonColliderDataStructure.finalPoints)
                {
                    body.CreatePolygonFixture(verxtPoint, B2DPolygonColliderDataStructure.isSensor,
                        unit);
                }

                break;
        }
    }

    public static void RenderAB2D_Collider(Unit ownerUnit, int B2D_ColliderDataConfigId, Vector2 worldOffset,
        Quaternion worldRotation, long sustainTime)
    {
        B2D_ColliderDataRepositoryComponent B2DColliderDataRepositoryComponent =
            ownerUnit.DomainScene().GetComponent<B2D_ColliderDataRepositoryComponent>();

        B2D_ColliderDataStructureBase B2DColliderDataStructureBase =
            B2DColliderDataRepositoryComponent.GetDataByColliderId(B2D_ColliderDataConfigId);

        List<Vector2> colliderPoints = new List<Vector2>();
        switch (B2DColliderDataStructureBase)
        {
            case B2D_BoxColliderDataStructure B2DBoxColliderDataStructure:

                break;
            case B2D_CircleColliderDataStructure B2DCircleColliderDataStructure:

                break;
            case B2D_PolygonColliderDataStructure B2DPolygonColliderDataStructure:

                break;
        }
    }
}