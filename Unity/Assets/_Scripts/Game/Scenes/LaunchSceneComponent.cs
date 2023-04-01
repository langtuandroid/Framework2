using System.Collections.Generic;
using Framework;
using NPBehave;
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
        self.DomainScene().AddComponent<NP_TreeDataRepositoryComponent>();
        var unitComponent = self.DomainScene().AddComponent<UnitComponent>();
        Unit unit = unitComponent.AddChild<Unit>();
        unit.Forward = new float3(0, 0, 1);
        unit.AddComponent<GameObjectComponent>().GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        unitComponent.Add(unit);
        var runtimeTreeMgr = unit.AddComponent<NP_RuntimeTreeManager>();
        long nPDataId = 105926695190565;
        var tree = NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, nPDataId);
        tree.Start();
    }
}