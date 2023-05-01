using System;
using System.Collections.Generic;
using Framework;

namespace ET
{
    /// <summary>
    /// 类似AIDispatcher，是全局的，整个进程只有一个，因为其本身就是一个无状态函数封装
    /// </summary>
    public class B2D_CollisionDispatcherComponent : Entity , IAwakeSystem
    {
        public static B2D_CollisionDispatcherComponent Instance;

        public Dictionary<string, AB2D_CollisionHandler> B2DCollisionHandlers =
            new Dictionary<string, AB2D_CollisionHandler>();

        /// <summary>
        /// 处理碰撞开始，a碰到了b
        /// </summary>
        public void HandleCollisionStart(Unit a, Unit b)
        {
            if (B2DCollisionHandlers.TryGetValue(a.GetComponent<B2D_ColliderComponent>().CollisionHandlerName,
                out var collisionHandler))
            {
                collisionHandler.HandleCollisionStart(a, b);
            }
        }

        /// <summary>
        /// 处理碰撞持续
        /// </summary>
        public void HandleCollisionSustain(Unit a, Unit b)
        {
            if (B2DCollisionHandlers.TryGetValue(a.GetComponent<B2D_ColliderComponent>().CollisionHandlerName,
                out var collisionHandler))
            {
                collisionHandler.HandleCollisionStay(a, b);
            }
        }

        /// <summary>
        /// 处理碰撞结束
        /// </summary>
        public void HandleCollsionEnd(Unit a, Unit b)
        {
            if (B2DCollisionHandlers.TryGetValue(a.GetComponent<B2D_ColliderComponent>().CollisionHandlerName,
                out var collisionHandler))
            {
                collisionHandler.HandleCollisionEnd(a, b);
            }
        }

        public void Awake()
        {
            B2DCollisionHandlers.Clear();

            Instance = this;

            var types = EventSystem.Instance.GetTypes(typeof(B2D_CollisionHandlerAttribute));
            foreach (Type type in types)
            {
                AB2D_CollisionHandler bAb2SCollisionHandler = Activator.CreateInstance(type) as AB2D_CollisionHandler;
                if (bAb2SCollisionHandler == null)
                {
                    Log.Error($"robot ai is not AB2S_CollisionHandler: {type.Name}");
                    continue;
                }

                B2DCollisionHandlers.Add(type.Name, bAb2SCollisionHandler);
            }
        }
    }
}