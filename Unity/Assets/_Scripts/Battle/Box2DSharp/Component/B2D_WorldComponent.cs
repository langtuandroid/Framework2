using System.Collections.Generic;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Framework;
using Unity.Mathematics;
using UnityEngine;
using Color = Box2DSharp.Common.Color;
using Transform = Box2DSharp.Common.Transform;
using Vector2 = System.Numerics.Vector2;

/// <summary>
/// 物理世界组件，代表一个物理世界
/// </summary>
public class B2D_WorldComponent : Entity, IAwakeSystem , IUpdateSystem
{
    private World m_World;

    public List<Body> BodyToDestroy = new List<Body>();

    public const int VelocityIteration = 10;
    public const int PositionIteration = 10;

    public void AddBodyTobeDestroyed(Body body)
    {
        BodyToDestroy.Add(body);
    }

    public void FixedUpdate()
    {
        foreach (var body in BodyToDestroy)
        {
            m_World.DestroyBody(body);
        }

        BodyToDestroy.Clear();
        this.m_World.Step(GlobalDefine.FixedUpdateTargetDTTime_Float, VelocityIteration, PositionIteration);
    }

    public World GetWorld()
    {
        return this.m_World;
    }

    public override void Dispose()
    {
        if (this.IsDisposed)
        {
            return;
        }

        base.Dispose();
        foreach (var body in BodyToDestroy)
        {
            m_World.DestroyBody(body);
        }

        BodyToDestroy.Clear();

        this.m_World.Dispose();
        this.m_World = null;
    }

    public void Awake()
    {
        this.m_World = B2D_WorldUtility.CreateWorld(new Vector2(0, 0));
       //  m_World.SetDebugDrawer(new Box2DDrawer());
    }


    public class Box2DDrawer : IDrawer
    {
        public DrawFlag Flags { get; set; } =
            DrawFlag.DrawShape | DrawFlag.DrawAABB | DrawFlag.DrawContactPoint | DrawFlag.DrawPair;

        public void DrawPolygon(Vector2[] vertices, int vertexCount, in Color color)
        {
            Gizmos.color = new UnityEngine.Color(color.R, color.G, color.B, color.A);
            for (int i = 0; i < vertexCount - 1; i++)
            {
                Gizmos.DrawLine(new float3(vertices[i].X, 0, vertices[i].Y),
                    new float3(vertices[i + 1].X, 0, vertices[i + 1].Y));
            }

            Gizmos.DrawLine(new float3(vertices[0].X, 0, vertices[0].Y),
                new float3(vertices[vertexCount - 1].X, 0, vertices[vertexCount - 1].Y));
        }

        public void DrawSolidPolygon(Vector2[] vertices, int vertexCount, in Color color)
        {
            DrawPolygon(vertices, vertexCount, color);
        }

        public void DrawCircle(in Vector2 center, float radius, in Color color)
        {
            Gizmos.color = new UnityEngine.Color(color.R, color.G, color.B, color.A);
            Gizmos.DrawSphere(new float3(center.X, 0, center.Y), radius);
        }

        public void DrawSolidCircle(in Vector2 center, float radius, in Vector2 axis, in Color color)
        {
            DrawCircle(center, radius, color);
        }

        public void DrawSegment(in Vector2 p1, in Vector2 p2, in Color color)
        {
            Gizmos.color = new UnityEngine.Color(color.R, color.G, color.B, color.A);
            Gizmos.DrawLine(new float3(p1.X, 0, p1.Y), new float3(p2.X, 0, p2.Y));
        }

        public void DrawTransform(in Transform xf)
        {
            Gizmos.color = UnityEngine.Color.green;
            Gizmos.DrawCube(new float3(xf.Position.X, 0, xf.Position.Y), new float3(1));
        }

        public void DrawPoint(in Vector2 p, float size, in Color color)
        {
            Gizmos.color = new UnityEngine.Color(color.R, color.G, color.B, color.A);
            Gizmos.DrawSphere(new float3(p.X, 0, p.Y), size);
        }
    }

    public void Update(float deltaTime)
    {
        FixedUpdate();
    }
}