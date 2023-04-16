using System;
using Framework;
using Unity.Mathematics;
using UnityEngine;

public class LaunchSceneComponent : Entity ,IAwakeSystem, IUpdateSystem, IRendererUpdateSystem
{
    public void Awake()
    {
        InstanceQueueMap.InstanceQueueMapDic[typeof(IBattleUpdateSystem)] = InstanceQueueIndex.BattleUpdate;
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

        UIComponent.Instance.OpenAsync<UI_UnitInfo>(new UI_UnitInfoVM());
    }

    public void Update(float deltaTime)
    {
        var queue = EventSystem.Instance.GetQueueByIndex(InstanceQueueIndex.BattleUpdate);
        int count = queue.Count;
        while (count-- > 0)
        {
            long instanceId = queue.Dequeue();
            Entity component = Root.Instance.Get(instanceId);
            if (component == null)
            {
                continue;
            }

            if (component.IsDisposed)
            {
                continue;
            }
            queue.Enqueue(instanceId);

            if (component is IBattleUpdateSystem iUpdateSystem)
            {
                try
                {
                    iUpdateSystem.BattleUpdate(deltaTime);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }

    public void RenderUpdate(float deltaTime)
    {
    }
}