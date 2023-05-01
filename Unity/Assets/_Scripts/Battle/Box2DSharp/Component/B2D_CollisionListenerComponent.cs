using System.Collections.Generic;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using Framework;

namespace ET
{

    /// <summary>
    /// 某一物理世界所有碰撞的监听者，负责碰撞事件的分发
    /// </summary>
    public class B2D_CollisionListenerComponent : Entity, IContactListener ,IAwakeSystem, IUpdateSystem
    {
        public B2D_WorldColliderManagerComponent B2DWorldColliderManagerComponent;

        private List<(long, long)> m_CollisionRecorder = new List<(long, long)>();

        private List<(long, long)> m_ToBeRemovedCollisionData = new List<(long, long)>();

        public void BeginContact(Contact contact)
        {
            //这里获取的是碰撞实体，比如诺克Q技能的碰撞体Unit，这里获取的就是它
            Unit unitA = (Unit) contact.FixtureA.UserData;
            Unit unitB = (Unit) contact.FixtureB.UserData;

            if (unitA.IsDisposed || unitB.IsDisposed)
            {
                return;
            }
            
            m_CollisionRecorder.Add((unitA.Id, unitB.Id));

            B2D_CollisionDispatcherComponent.Instance.HandleCollisionStart(unitA, unitB);
            B2D_CollisionDispatcherComponent.Instance.HandleCollisionStart(unitB, unitA);
        }

        public void EndContact(Contact contact)
        {
            Unit unitA = (Unit) contact.FixtureA.UserData;
            Unit unitB = (Unit) contact.FixtureB.UserData;
            
            // Id不分顺序，防止移除失败
            this.m_ToBeRemovedCollisionData.Add((unitA.Id, unitB.Id));
            this.m_ToBeRemovedCollisionData.Add((unitB.Id, unitA.Id));

            if (unitA.IsDisposed || unitB.IsDisposed)
            {
                return;
            }
            
            B2D_CollisionDispatcherComponent.Instance.HandleCollsionEnd(unitA, unitB);
            B2D_CollisionDispatcherComponent.Instance.HandleCollsionEnd(unitB, unitA);
        }

        public void PreSolve(Contact contact, in Manifold oldManifold)
        {
        }

        public void PostSolve(Contact contact, in ContactImpulse impulse)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.IsDisposed)
                return;
            m_ToBeRemovedCollisionData.Clear();
            this.m_CollisionRecorder.Clear();
        }

        public void Awake()
        {
            //绑定指定的物理世界，正常来说一个房间一个物理世界,这里是Demo，直接获取了
            Parent.GetComponent<B2D_WorldComponent>().GetWorld().SetContactListener(this);
            //self.TestCollision();
            B2DWorldColliderManagerComponent = Parent.GetComponent<B2D_WorldColliderManagerComponent>();
        }

        public void Update(float deltaTime)
        {
            foreach (var tobeRemovedData in m_ToBeRemovedCollisionData)
            {
                this.m_CollisionRecorder.Remove(tobeRemovedData);
            }

            m_ToBeRemovedCollisionData.Clear();
            
            foreach (var cachedCollisionData in this.m_CollisionRecorder)
            {
                Unit unitA = this.DomainScene().GetComponent<UnitComponent>().Get(cachedCollisionData.Item1);
                Unit unitB = this.DomainScene().GetComponent<UnitComponent>().Get(cachedCollisionData.Item2);
                
                if (unitA == null || unitB == null || unitA.IsDisposed || unitB.IsDisposed)
                {
                    // Id不分顺序，防止移除失败
                    this.m_ToBeRemovedCollisionData.Add((cachedCollisionData.Item1, cachedCollisionData.Item2));
                    this.m_ToBeRemovedCollisionData.Add((cachedCollisionData.Item2, cachedCollisionData.Item1));
                    continue;
                }

                B2D_CollisionDispatcherComponent.Instance.HandleCollisionSustain(unitA, unitB);
                B2D_CollisionDispatcherComponent.Instance.HandleCollisionSustain(unitB, unitA);
            }
        }
    }
}