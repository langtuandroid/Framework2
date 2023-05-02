using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Dynamics;
using Framework;

/// <summary>
/// 物理世界组件，代表一个物理世界
/// </summary>
public class B2D_WorldComponent : Entity, IAwakeSystem
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
    }
}