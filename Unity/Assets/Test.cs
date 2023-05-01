using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using ET;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Color = Box2DSharp.Common.Color;
using Random = UnityEngine.Random;
using Transform = Box2DSharp.Common.Transform;
using Vector2 = System.Numerics.Vector2;

public class Test : MonoBehaviour
{
    private World world;
    [Button]
    async void Start()
    {
        world = new World(new Vector2(0,-1));
        world.SetDebugDrawer(new Box2DDrawer());
        var body =world.CreateBody(new BodyDef() { BodyType = BodyType.StaticBody });
        body.CreateBoxFixture(10,1, new float2(0),0,false,null);
        body.SetTransform(new Vector2(0, 0), 0);
        body.SetMassData(new MassData() { Center = body.GetPosition(), Mass = 1 });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var body = world.CreateBody(new BodyDef() { AllowSleep = false, BodyType = Random.Range(1,11) > 5 ? BodyType.StaticBody : BodyType.DynamicBody});
            body.CreateCircleFixture(1, new float2(0), false, null);
            body.SetTransform(new Vector2(Random.Range(1,10), Random.Range(1,10)), 0);
            body.SetMassData(new MassData(){Center = body.GetPosition(), Mass = 1});
        }
    }

    private void LateUpdate()
    {
        world.Step(Time.fixedDeltaTime,10,10);
    }

    private void OnDrawGizmos()
    {
        if (world == null) return;
        world.DebugDraw();
    }
}

public class Box2DDrawer : IDrawer
{
    public DrawFlag Flags { get; set; } =
        DrawFlag.DrawShape | DrawFlag.DrawAABB | DrawFlag.DrawContactPoint | DrawFlag.DrawPair;
    public void DrawPolygon(Vector2[] vertices, int vertexCount, in Color color)
    {
        Gizmos.color = new UnityEngine.Color(color.R,color.G,color.B,color.A);
        for (int i = 0; i < vertexCount - 1; i++)
        {
            Gizmos.DrawLine(new Vector3(vertices[i].X, 0, vertices[i].Y),
                new Vector3(vertices[i + 1].X, 0, vertices[i + 1].Y));
        }
        Gizmos.DrawLine(new Vector3(vertices[0].X, 0, vertices[0].Y),
            new Vector3(vertices[vertexCount - 1].X, 0, vertices[vertexCount - 1].Y));
    }

    public void DrawSolidPolygon(Vector2[] vertices, int vertexCount, in Color color)
    {
        DrawPolygon(vertices, vertexCount, color);
    }

    public void DrawCircle(in Vector2 center, float radius, in Color color)
    {
        Gizmos.color = new UnityEngine.Color(color.R,color.G,color.B,color.A);
        Gizmos.DrawSphere(new Vector3(center.X,0,center.Y), radius);
    }

    public void DrawSolidCircle(in Vector2 center, float radius, in Vector2 axis, in Color color)
    {
        DrawCircle(center, radius, color);
    }

    public void DrawSegment(in Vector2 p1, in Vector2 p2, in Color color)
    {
        Gizmos.color = new UnityEngine.Color(color.R,color.G,color.B,color.A);
        Gizmos.DrawLine(new Vector3(p1.X, 0, p1.Y), new Vector3(p2.X, 0, p2.Y));
    }

    public void DrawTransform(in Transform xf)
    {
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawCube(new Vector3(xf.Position.X,0,xf.Position.Y), Vector3.one);
    }

    public void DrawPoint(in Vector2 p, float size, in Color color)
    {
        Gizmos.color = new UnityEngine.Color(color.R,color.G,color.B,color.A);
        Gizmos.DrawSphere(new Vector3(p.X,0,p.Y), size);
    }
}