using System.Collections.Generic;
using Box2DSharp.Dynamics;
using Framework;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

/// <summary>
/// 物理世界组件，代表一个物理世界
/// </summary>
public class B2D_WorldComponent : Entity, IAwakeSystem, IFixedUpdateSystem , IDestroySystem
{
    private World m_World;
    private Box2DDrawer drawer;

    private List<Body> BodyToDestroy = new List<Body>();

    public const int VelocityIteration = 10;
    public const int PositionIteration = 10;
    
    public void Awake()
    {
        this.m_World = B2D_WorldUtility.CreateWorld(new Vector2(0, 0));
        drawer = new GameObject("Box2dDrawer").AddComponent<Box2DDrawer>();
        Object.DontDestroyOnLoad(drawer.gameObject);
        drawer.World = m_World;
        m_World.Drawer = drawer;
    }

    public void AddBodyTobeDestroyed(Body body)
    {
        BodyToDestroy.Add(body);
    }

    public void FixedUpdate(float deltaTime)
    {
        foreach (var body in BodyToDestroy)
        {
            m_World.DestroyBody(body);
        }

        BodyToDestroy.Clear();
        this.m_World.Step(deltaTime, VelocityIteration, PositionIteration);
    }

    public World GetWorld()
    {
        return this.m_World;
    }

    public void OnDestroy()
    {
        Object.Destroy(drawer.gameObject);
        foreach (var body in BodyToDestroy)
        {
            m_World.DestroyBody(body);
        }

        BodyToDestroy.Clear();

        this.m_World.Dispose();
        this.m_World = null;
    }
}