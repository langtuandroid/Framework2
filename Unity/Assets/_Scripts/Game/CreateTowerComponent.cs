using Framework;
using UnityEngine;

[Event(SceneType.Battle)]
public class CreateTowerEvent : AEvent<TowerCreateData>
{
    protected override async ETTask Run(Scene scene, TowerCreateData a)
    {
        scene.GetComponent<CreateTowerComponent>().CreateTower(a);
        await ETTask.CompletedTask;
    }
}

public struct TowerCreateData
{
    public TowerData TowerData;
    public Vector3 Pos;
    public Quaternion Rotation;
}

public class CreateTowerComponent : Entity, IAwakeSystem, IRendererUpdateSystem
{
    public void Awake()
    {
    }

    public void RenderUpdate(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreatePreview(1);
        }
    }

    private async void CreatePreview(int towerId)
    {
        TowerData data = TowerData.Get(towerId);
        var go = await ResComponent.Instance.InstantiateAsync(data.PreviewPath);
        var unit = UnitFactory.CreateTower(this.DomainScene());
        unit.GetComponent<GameObjectComponent>().GameObject = go;
        var preview = unit.AddComponent<TowerPreview, TowerData>(data);
        preview.Show();
    }

    public async void CreateTower(TowerCreateData createData)
    {
        var go = await ResComponent.Instance.InstantiateAsync(createData.TowerData.LevelDatas[0].ModelPath);
        var unit = UnitFactory.CreateTower(this.DomainScene());
        unit.Position = createData.Pos;
        unit.Rotation = createData.Rotation;
        unit.GetComponent<GameObjectComponent>().GameObject = go;
        unit.GetComponent<GameObjectComponent>().GameObject.name = "player";
        NP_RuntimeTree skill = NP_RuntimeTreeFactory.CreateSkillRuntimeTree(unit, 10002);
        unit.GetComponent<SkillCanvasManagerComponent>().AddSkill(10002);
        skill.Start();
    }
}