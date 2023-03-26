using Framework;
using Unity.Mathematics;
using UnityEngine;

public class LaunchSceneComponent : Entity ,IAwake
{
}

public class LaunchSceneComponentAwakeSystem : AwakeSystem<LaunchSceneComponent>
{
    protected override void Awake(LaunchSceneComponent self)
    {
        Log.Msg(self.DomainScene().Name);
        var unitComponent = self.AddComponent<UnitComponent>();
        Unit unit = unitComponent.AddChild<Unit>();
        unit.Forward = new float3(0, 0, 1);
        unit.AddComponent<GameObjectComponent>().GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        unitComponent.Add(unit);
        unit.AddComponent<AMoveComponent>();
    }
}