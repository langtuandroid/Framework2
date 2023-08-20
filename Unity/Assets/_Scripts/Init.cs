using System;
using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class Init : MonoBehaviour
{

    [Button]
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);
        AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };
        Game.Close();
        FillTowerData();
        Game.AddSingleton<EventSystem>();
        EventSystem.Instance.Add(AssemblyHelper.GetAssemblyTypes(typeof(Game).Assembly));
        EventSystem.Instance.Add(AssemblyHelper.GetAssemblyTypes(typeof(Init).Assembly));
        EventSystem.Instance.InitType();
#if !UNITY_EDITOR
        MongoHelper.Init();
        MongoGameRegister.RegisterStruct();
#endif
        Game.AddSingleton<MainThreadSynchronizationContext>();
        Game.AddSingleton<CameraComponent>();
        Game.AddSingleton<TimeInfo>();
        Game.AddSingleton<IdGenerator>();
        Game.AddSingleton<TimerComponent>();
        Game.AddSingleton<CoroutineLockComponent>();
        Game.AddSingleton<CollisionHandlerCollector>();
        var root = Game.AddSingleton<Root>();
        Game.AddSingleton<ResComponent>();
        Game.AddSingleton<ConfigComponent>();
        root.Scene.AddComponent<GlobalReferenceComponent>();
        root.Scene.AddComponent<UIComponent>();
        await ResComponent.Instance.Init();
        await ConfigComponent.Instance.LoadAsync();
        ETTask.ExceptionHandler += e => Log.Error(e);
        var currentScene = Root.Instance.Scene.AddComponent<CurrentScenesComponent>();
        currentScene.ChangeScene(SceneType.Launch);
    }

    private void FillTowerData()
    {
        TowerData.Data = new Dictionary<int, TowerData>()
        {
            {
                1, new TowerData()
                {
                    Id = 1,
                    PreviewPath = "Assets/Res/Towers/AssaultCannon/AssaultCannonPreview.prefab",
                    Dimensions = new int2(2, 2),
                    LevelDatas = new[]
                    {
                        new TowerLevelData()
                        {
                            Id = 0,
                            ModelPath = "Assets/Res/Towers/AssaultCannon/AssaultCannon_Level1.prefab",
                            BuildEnergy = 10,
                            SellEnergy = 7,
                            Range = 3,
                        },
                        new TowerLevelData()
                        {
                            Id = 1,
                            ModelPath = "Assets/Res/Towers/AssaultCannon/AssaultCannon_Level2.prefab",
                            BuildEnergy = 15,
                            SellEnergy = 12,
                            Range = 4,
                        },
                        new TowerLevelData()
                        {
                            Id = 2,
                            ModelPath = "Assets/Res/Towers/AssaultCannon/AssaultCannon_Level3.prefab",
                            BuildEnergy = 20,
                            SellEnergy = 17,
                            Range = 5,
                        },
                    }
                }
            }
        };
    }
    
    /// <summary>
    /// 按每秒15帧的频率更新逻辑帧
    /// </summary>
    private float logicUpdateInterval = 1 / 15f;
    private float logicUpdateTimer = 1 / 15f;
    private void Update()
    {
        logicUpdateTimer += Time.deltaTime;
        if (logicUpdateTimer >= logicUpdateInterval)
        {
            Game.Update(logicUpdateTimer);
            logicUpdateTimer = 0;
        }

        Game.RendererUpdate(Time.deltaTime);
    }

    private void LateUpdate()
    {
        Game.LateUpdate(Time.deltaTime);
        Game.FrameFinishUpdate();
    }

    private void FixedUpdate()
    {
        Game.FixedUpdate(Time.fixedTime);
    }

    public void OnGUI()
    {
        
    }

    private void OnApplicationQuit()
    {
        Game.Close();
    }
}