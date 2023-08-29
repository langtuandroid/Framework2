using System.Collections.Generic;
using Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = Framework.Scene;

public class BattleSceneComponent : Entity, IAwakeSystem, IUpdateSystem, IRendererUpdateSystem
{
    public async void Awake()
    {
        InstanceQueueMap.InstanceQueueMapDic[typeof(IBattleUpdateSystem)] = InstanceQueueIndex.BattleUpdate;

        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        await TimerComponent.Instance.WaitAsync(1000);
        Scene battleScene = this.DomainScene();
        battleScene.AddComponent<UnitComponent>();
        battleScene.AddComponent<AutoGenEnemyComponent>();
        battleScene.AddComponent<CDComponent>();
        battleScene.AddComponent<BattleEventSystemComponent>();
        battleScene.AddComponent<NumericWatcherComponent>();
        battleScene.AddComponent<CreateTowerComponent>();

        // Unit unit2 = UnitFactory.CreateHero(battleScene, RoleCamp.enemy);
        // unit2.GetComponent<GameObjectComponent>().GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // unit2.Position = new float3(3, 0, 3);
        // var aiBehave = NP_RuntimeTreeFactory.CreateBehaveRuntimeTree(unit2, 10001);
        // aiBehave.Start();
        // var skillBehave = NP_RuntimeTreeFactory.CreateSkillRuntimeTree(unit2, 10001);
        // unit2.GetComponent<SkillCanvasManagerComponent>().AddSkill(10001);
        // skillBehave.Start();
        //
        // Unit unit = UnitFactory.CreateHero(battleScene, RoleCamp.self);
        // unit.AddComponent<KeyboardCtrlComponent>();
        // unit.Forward = new float3(0, 0, 1);
        // unit.GetComponent<GameObjectComponent>().GameObject =
        //     ResComponent.Instance.Instantiate("Assets/Res/Player.prefab");
        //
        // unit.GetComponent<GameObjectComponent>().GameObject.name = "player";
        // NP_RuntimeTree skill = NP_RuntimeTreeFactory.CreateSkillRuntimeTree(unit, 10002);
        // unit.GetComponent<SkillCanvasManagerComponent>().AddSkill(10002);
        // skill.Start();
        // UIComponent.Instance.OpenAsync<UI_UnitInfo>(new UI_UnitInfoVM(unit));
    }

    public void Update(float deltaTime)
    {
        Queue<long> queue = EventSystem.Instance.GetQueueByIndex(InstanceQueueIndex.BattleUpdate);
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
                iUpdateSystem.BattleUpdate(deltaTime);
            }
        }
    }

    public void RenderUpdate(float deltaTime)
    {
    }
}