using Framework;

public class DefaultCollisionHandler: AB2D_CollisionHandler
{
    public override void HandleCollisionStart(ColliderUserData a, ColliderUserData b)
    {
        
    }

    public override void HandleCollisionStay(ColliderUserData a, ColliderUserData b)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleCollisionEnd(ColliderUserData a, ColliderUserData b)
    {
        throw new System.NotImplementedException();
    }
}