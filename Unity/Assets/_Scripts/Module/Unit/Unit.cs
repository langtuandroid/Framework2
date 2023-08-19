using System.Diagnostics;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace Framework
{
    public sealed class Unit : Entity, IAwakeSystem
    {
        private float3 position; //坐标

        [ShowInInspector]
        public float3 Position
        {
            get => position;
            set => position = value;
        }

        public float3 Forward
        {
            get => math.mul(Rotation, math.forward());
            set => Rotation = quaternion.LookRotation(value, math.up());
        }

        private quaternion rotation;

        public quaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                eulerAngle = rotation.EulerAngles();
            }
        }

        private float3 eulerAngle;

        [ShowInInspector]
        public float3 EulerAngle
        {
            get => eulerAngle;
            set
            {
                eulerAngle = value;
                rotation = quaternion.Euler(eulerAngle, math.RotationOrder.XYZ);
            }
        }

        private float3 scale;

        [ShowInInspector]
        public float3 Scale
        {
            get => scale;
            set => scale = value;
        }

        protected override string ViewName => $"{GetType().Name} ({Id})";

        public void Awake()
        {
  
        }

        public override string ToString()
        {
            return $"{GetComponent<GameObjectComponent>()?.GameObject.name} {base.ToString()}";
        }
    }
}