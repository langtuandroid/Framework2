﻿using System.Diagnostics;
using Unity.Mathematics;

namespace Framework
{
    [DebuggerDisplay("ViewName,nq")]
    public class Unit : Entity, IAwakeSystem
    {
    
        public void Awake(Entity o)
        {
            
        }
        
        private float3 position; //坐标

        public float3 Position
        {
            get => this.position;
            set
            {
                float3 oldPos = this.position;
                this.position = value;
                EventSystem.Instance.Publish(this.DomainScene(),
                    new EventType.ChangePosition() { Unit = this, OldPos = oldPos });
            }
        }

        public float3 Forward
        {
            get => math.mul(this.Rotation, math.forward());
            set => this.Rotation = quaternion.LookRotation(value, math.up());
        }

        private quaternion rotation;

        public quaternion Rotation
        {
            get => this.rotation;
            set
            {
                this.rotation = value;
                EventSystem.Instance.Publish(this.DomainScene(), new EventType.ChangeRotation() { Unit = this });
            }
        }

        protected override string ViewName
        {
            get { return $"{this.GetType().Name} ({this.Id})"; }
        }


    }
}