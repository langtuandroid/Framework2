using System;
using System.Collections.Generic;
using Framework;

public class CollisionHandlerCollector : Singleton<CollisionHandlerCollector>, ISingletonAwake
{
    private Dictionary<string, ACollisionHandler> collisionHandlers = new Dictionary<string, ACollisionHandler>();
    
    public void Awake()
    {
        var types = EventSystem.Instance.GetTypes(typeof(B2D_CollisionHandlerAttribute));
        foreach (Type type in types)
        {
            ACollisionHandler collisionHandler = Activator.CreateInstance(type) as ACollisionHandler;
            if (collisionHandler == null)
            {
                Log.Error($"robot ai is not AB2S_CollisionHandler: {type.Name}");
                continue;
            }

            collisionHandlers.Add(type.Name, collisionHandler);
        }
    }
    
    /// <summary>
    /// 处理碰撞开始，a碰到了b
    /// </summary>
    public void HandleCollisionStart(ColliderUserData a, ColliderUserData b)
    {
        if (collisionHandlers.TryGetValue(a.Unit.GetComponent<ColliderComponent>().CollisionHandlerName,
                out var collisionHandler))
        {
            collisionHandler.HandleCollisionStart(a, b);
        }
    }

    /// <summary>
    /// 处理碰撞持续
    /// </summary>
    public void HandleCollisionSustain(ColliderUserData a, ColliderUserData b)
    {
        if (collisionHandlers.TryGetValue(a.Unit.GetComponent<ColliderComponent>().CollisionHandlerName,
                out var collisionHandler))
        {
            collisionHandler.HandleCollisionStay(a, b);
        }
    }

    /// <summary>
    /// 处理碰撞结束
    /// </summary>
    public void HandleCollsionEnd(ColliderUserData a, ColliderUserData b)
    {
        if (collisionHandlers.TryGetValue(a.Unit.GetComponent<ColliderComponent>().CollisionHandlerName,
                out var collisionHandler))
        {
            collisionHandler.HandleCollisionEnd(a, b);
        }
    }

}