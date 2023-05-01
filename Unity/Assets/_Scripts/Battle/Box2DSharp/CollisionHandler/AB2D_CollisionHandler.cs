﻿using Framework;

namespace ET
{
    public class B2D_CollisionHandlerAttribute : BaseAttribute
    {
    }

    [B2D_CollisionHandler]
    public abstract class AB2D_CollisionHandler
    {
        /// <summary>
        /// a是碰撞者自身，b是碰撞到的目标
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public abstract void HandleCollisionStart(Unit a, Unit b);

        /// <summary>
        /// a是碰撞者自身，b是碰撞到的目标
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public abstract void HandleCollisionStay(Unit a, Unit b);

        /// <summary>
        /// a是碰撞者自身，b是碰撞到的目标
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public abstract void HandleCollisionEnd(Unit a, Unit b);
    }
}