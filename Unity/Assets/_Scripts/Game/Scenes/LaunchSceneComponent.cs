﻿using Framework;
using Unity.Mathematics;
using UnityEngine;

public class LaunchSceneComponent : Entity ,IAwakeSystem
{
    public void Awake()
    {
        Log.Msg(this.DomainScene().Name);
        this.DomainScene().AddComponent<NP_TreeDataRepositoryComponent>();
        var unitComponent = this.DomainScene().AddComponent<UnitComponent>();
        Unit unit = unitComponent.AddChild<Unit>();
        unit.AddComponent<NumericComponent>();
        unit.AddComponent<BuffManagerComponent>();
        unit.AddComponent<CastDamageComponent>();
        unit.AddComponent<ReceiveDamageComponent>();
        unit.Forward = new float3(0, 0, 1);
        unit.AddComponent<GameObjectComponent>().GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        unitComponent.Add(unit);
        unit.AddComponent<NP_RuntimeTreeManager>();
        long nPDataId = 105926695190565;
        var tree = NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, nPDataId);
        tree.Start();
    }
}