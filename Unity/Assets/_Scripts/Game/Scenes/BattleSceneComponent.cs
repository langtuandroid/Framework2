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
        battleScene.AddComponent<B2D_WorldComponent>();
        battleScene.AddComponent<B2D_ColliderDataRepositoryComponent>();
        battleScene.AddComponent<B2D_CollisionListenerComponent>();
        battleScene.AddComponent<B2D_WorldColliderManagerComponent>();
        battleScene.AddComponent<UnitComponent>();
        
        Unit unit2 = UnitFactory.CreateHero(battleScene, RoleCamp.bule, 1);
        unit2.GetComponent<GameObjectComponent>().GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        unit2.Position = new float3(3, 0, 3);

        Unit unit = UnitFactory.CreateHero(battleScene, RoleCamp.red, 1);
        unit.Forward = new float3(0, 0, 1);
        unit.GetComponent<GameObjectComponent>().GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        unit.GetComponent<GameObjectComponent>().GameObject.name = "player";
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