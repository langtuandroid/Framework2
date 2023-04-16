using System;
using System.Collections.Generic;
using System.IO;
using ET;
using Framework;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

public class Init : MonoBehaviour
{

    [Button]
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);
        AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };
        Game.Close();

        Game.AddSingleton<EventSystem>();
        EventSystem.Instance.Add(AssemblyHelper.GetAssemblyTypes(typeof(Game).Assembly));
        EventSystem.Instance.Add(AssemblyHelper.GetAssemblyTypes(typeof(Init).Assembly));
        EventSystem.Instance.InitType();
        Game.AddSingleton<MainThreadSynchronizationContext>();
        Game.AddSingleton<TimeInfo>();
        Game.AddSingleton<ObjectPool>();
        Game.AddSingleton<IdGenerator>();
        Game.AddSingleton<TimerComponent>();
        Game.AddSingleton<CoroutineLockComponent>();
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

    private void OnApplicationQuit()
    {
        Game.Close();
    }
}