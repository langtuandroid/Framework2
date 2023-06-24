using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

namespace Framework
{
    public sealed class Unit : Entity, IAwakeSystem<int>
    {
    
        private float3 position; //坐标
        
        public UnitConfig UnitConfig { get; private set; }

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
                eulerAngle = rotation.EulerAngles();
                EventSystem.Instance.Publish(this.DomainScene(), new EventType.ChangeRotation() { Unit = this });
            }
        }

        private float3 eulerAngle;

        public float3 EulerAngle
        {
            get => eulerAngle;
            set
            {
                if(eulerAngle.NearEqual(value)) return;
                eulerAngle = value;
                rotation = quaternion.Euler(eulerAngle, math.RotationOrder.XYZ);
                EventSystem.Instance.Publish(this.DomainScene(), new EventType.ChangeRotation() { Unit = this });
            }
        }

        private float3 scale;

        public float3 Scale
        {
            get => scale;
            set
            {
                scale = value;
                EventSystem.Instance.Publish(this.DomainScene(), new EventType.ChangeScale() { Unit = this });
            }
        }

        protected override string ViewName
        {
            get
            {
                return $"{this.GetType().Name} ({this.Id})";
            }
        }

        public void Awake(int unitId)
        {
            if (UnitConfigFactory.Instance.Contain(unitId))
                UnitConfig = UnitConfigFactory.Instance.Get(unitId);
        }

        public override string ToString()
        {
            return $"{GetComponent<GameObjectComponent>()?.GameObject.name} {base.ToString()}";
        }
    }
}