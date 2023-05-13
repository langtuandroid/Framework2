using System.Collections.Generic;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Color = Box2DSharp.Common.Color;
using Random = UnityEngine.Random;
using Transform = Box2DSharp.Common.Transform;
using Vector2 = System.Numerics.Vector2;

public class Test : MonoBehaviour
{
    private World world;
    [Button]
    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var body = world.CreateBody(new BodyDef() { AllowSleep = false, BodyType = Random.Range(1,11) > 5 ? BodyType.StaticBody : BodyType.DynamicBody});
            body.CreateCircleFixture(1, new float2(0), false, null);
            body.SetTransform(new Vector2(Random.Range(1,10), Random.Range(1,10)), 0);
            body.SetMassData(new MassData(){Center = body.GetPosition(), Mass = 1});
        }
    }

    private void LateUpdate()
    {
        world.Step(Time.fixedDeltaTime,10,10);
    }

    private void OnDrawGizmos()
    {
        if (world == null) return;
        world.DebugDraw();
    }
}
