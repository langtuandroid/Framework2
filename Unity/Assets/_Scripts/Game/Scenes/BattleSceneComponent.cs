using System;
using Framework;
using Unity.Mathematics;
using UnityEngine;

public class BattleSceneComponent : Entity ,IAwakeSystem, IUpdateSystem, IRendererUpdateSystem
{
    public void Awake()
    {
        InstanceQueueMap.InstanceQueueMapDic[typeof(IBattleUpdateSystem)] = InstanceQueueIndex.BattleUpdate;

        Scene battleScene = this.DomainScene();
        battleScene.AddComponent<NP_TreeDataRepositoryComponent>();
        battleScene.AddComponent<B2D_ColliderDataRepositoryComponent>();
        battleScene.AddComponent<B2D_CollisionListenerComponent>();
        battleScene.AddComponent<B2D_WorldColliderManagerComponent>();
        battleScene.AddComponent<B2D_WorldComponent>();
        
        var unitComponent = battleScene.AddComponent<UnitComponent>();
        Unit unit = unitComponent.AddChild<Unit>();
        unit.AddComponent<NumericComponent>();
        unit.AddComponent<BuffManagerComponent>();
        unit.AddComponent<CastDamageComponent>();
        unit.AddComponent<ReceiveDamageComponent>();
        unit.Forward = new float3(0, 0, 1);
        unit.AddComponent<GameObjectComponent>().GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        unitComponent.Add(unit);
        unit.AddComponent<NP_RuntimeTreeManager>();
        unit.AddComponent<MoveComponent>();
        long nPDataId = 105926695190565;
        var tree = NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, nPDataId);
        tree.Start();

        UIComponent.Instance.OpenAsync<UI_UnitInfo>(new UI_UnitInfoVM(unit));
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