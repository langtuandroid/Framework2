using Unity.Mathematics;

namespace Framework
{
    namespace EventType
    {
        public struct ChangePosition
        {
            public Unit Unit;
            public float3 OldPos;
        }

        public struct ChangeRotation
        {
            public Unit Unit;
        }
    }
}