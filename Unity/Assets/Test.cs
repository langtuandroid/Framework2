using System.Collections.Generic;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using NPBehave;
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
    private Clock clock;
    [Button]
    void Start()
    {
        clock = new Clock();
    }

    private void Update()
    {
        clock.Update(Time.deltaTime);
    }
}
