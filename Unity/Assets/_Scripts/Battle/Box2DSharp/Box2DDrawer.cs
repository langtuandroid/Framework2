using System;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Unity.Mathematics;
using UnityEngine;
using Color = Box2DSharp.Common.Color;
using Transform = Box2DSharp.Common.Transform;
using Vector2 = System.Numerics.Vector2;

public class Box2DDrawer : MonoBehaviour,IDrawer
{
    public DrawFlag Flags { get; set; } =
        DrawFlag.DrawShape | DrawFlag.DrawAABB | DrawFlag.DrawContactPoint | DrawFlag.DrawPair;

    public World World;

    private void OnDrawGizmos()
    {
        if (World != null)
        {
            World.DebugDraw();
        }
    }

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