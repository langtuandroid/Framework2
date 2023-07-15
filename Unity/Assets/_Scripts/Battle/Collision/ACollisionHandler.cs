using Framework;

public class B2D_CollisionHandlerAttribute : BaseAttribute
{
}

[B2D_CollisionHandler]
public abstract class ACollisionHandler
{
    /// <summary>
    /// a是碰撞者自身，b是碰撞到的目标
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public abstract void HandleCollisionStart(ColliderUserData a, ColliderUserData b);

    /// <summary>
    /// a是碰撞者自身，b是碰撞到的目标
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public abstract void HandleCollisionStay(ColliderUserData a, ColliderUserData b);

    /// <summary>
    /// a是碰撞者自身，b是碰撞到的目标
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public abstract void HandleCollisionEnd(ColliderUserData a, ColliderUserData b);
}