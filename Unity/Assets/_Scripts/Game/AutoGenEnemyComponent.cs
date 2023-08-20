using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AutoGenEnemyComponent : Entity, IAwakeSystem, IUpdateSystem
{
    private Transform startPos;
    private Transform endPos;
    private List<NavMeshAgent> units;
    private float timer;

    public void Awake()
    {
        var rootGos = SceneManager.GetSceneByName("Level1").GetRootGameObjects();
        startPos = rootGos.First(g => g.name == "StartPos").transform;
        endPos = rootGos.First(g => g.name == "EndPos").transform;
        units = new List<NavMeshAgent>();
    }

    public void Update(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= 2)
        {
            GenEnemy();
            timer = 0;
        }

        for (int i = 0; i < units.Count; i++)
        {
            var moveCom = units[i];
            if (moveCom == null)
            {
                units.RemoveAt(i);
                i--;
                continue;
            }

            if (moveCom.remainingDistance <= 0.1f)
            {
                Debug.Log($"pos:{moveCom.transform.position}  target:{moveCom.destination}  remain:{moveCom.remainingDistance}");
                moveCom.gameObject.DestroySelf();
                this.DomainScene().GetComponent<UnitComponent>().Get(moveCom.GetComponent<GoConnectedUnitId>().UnitId)
                    .Dispose();
                units.RemoveAt(i);
                i--;
            }
        }
    }

    private int index = 0;
    private async void GenEnemy()
    {
        var go = await ResComponent.Instance.InstantiateAsync("Assets/Res/Enemies/Hoverbuggy.prefab");
        var unit = UnitFactory.CreateSoldier(this.DomainScene(), RoleCamp.enemy);
        unit.Position = startPos.position;
        go.transform.position = startPos.position;
        go.name = $"enemy:{index++}";
        unit.GetComponent<GameObjectComponent>().GameObject = go;
        NavMeshAgent agent = go.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.SetDestination(endPos.position);
        agent.speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
        await TimerComponent.Instance.WaitAsync(500);
        units.Add(agent);
    }
}